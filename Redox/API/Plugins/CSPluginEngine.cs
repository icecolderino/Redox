using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using Redox.API;
using Redox.API.Libraries;
using Redox.Core.PluginEngines;
using Redox.Core.Plugins;

namespace Redox.API.Plugins
{
    public sealed class CSPluginEngine : IPluginEngine
    {
        public enum ViolationType : short
        {
            None = 0x01,
            Resources = 0x02,
            ForbiddenTypes = 0x03,
            UntrustedAssembly = 0x04

        }

        private static readonly Dictionary<string, Assembly> Plugins = new Dictionary<string, Assembly>();
        private static string path = Redox.PluginPath;

        public string Language => "CSharp";
        public string Pattern => "*.dll";

        public Dictionary<string, object> Values => new Dictionary<string, object>();

        public List<Assembly> Assemblies => new List<Assembly>();

        private static ILogger logger;

        public CSPluginEngine()
        {
            logger = Redox.Logger;
        }

        public void LoadPlugins()
        {
            logger.LogInfo("[CSharp] Loading plugins..");

            foreach (var dir in Directory.GetDirectories(path))
                LoadPlugin(dir);
        }

        public void LoadPlugin(string dir)
        {
            FileInfo info = null;

            foreach (var file in Directory.GetFiles(dir, Pattern))
            {
                try
                {

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    info = new FileInfo(file);
                    string name = info.Name.Replace(".dll", string.Empty);
                    Assembly assembly;

                    if (PluginCollector.GetCollector().GetContainer(name) == null)
                    {
                        if (!Plugins.TryGetValue(name, out assembly))
                        {
                            assembly = Assembly.Load(File.ReadAllBytes(file));
                            Plugins.Add(name, assembly);
                        }
                        ViolationType violationType;
                        if (this.IsSecure(assembly, out violationType))
                        {
                            foreach (Type type in assembly.GetExportedTypes())
                            {
                                if (type.IsSubclassOf(typeof(RedoxPlugin)) && type.IsPublic && !type.IsAbstract)
                                {
                                    object instance = Activator.CreateInstance(type);
                                    RedoxPlugin plugin = (RedoxPlugin)instance;

                                    if (((plugin.CoreVersion.ToString() == "0.0.0.0") || (plugin.CoreVersion >= Redox.version)) || Redox.config.LoadIncompitablePlugins)
                                    {
                                        PluginContainer container = new PluginContainer(plugin, instance, Language);
                                        container.Plugin.FileInfo = info;
                                        PluginCollector.GetCollector().AddPlugin(container);
                                        logger.LogInfo(string.Format("[CSharp] Succesfully loaded plugin {0}, {1}, Author {2} ({3})", plugin.Title, plugin.Version, plugin.Author, plugin.Description));
                                    }
                                    else
                                        logger.LogWarning($"[Redox] Plugin \"{plugin.Title}\" is not compitable with the current redox version!");
                                    

                                }
                            }
                            sw.Stop();
                            int time = sw.Elapsed.Milliseconds;

                            if (time > 500)
                                logger.LogSpeed(string.Format("[CSharp] Plugin {0} took {1} milliseconds to load", name, time));
                        }
                        else
                            logger.LogWarning(string.Format("[CSharp] {0} has been blocked due security violation: {1}", assembly.GetName().Name, violationType));
                    }
                    else
                    {
                        var container = PluginCollector.GetCollector().GetContainer(name);

                        if(!container.Running)
                        {
                            container.Start();
                            logger.LogInfo(string.Format("[CSharp] Succesfully loaded plugin {0}, {1}, Author {2} ({3})", container.Plugin.Title, container.Plugin.Version, container.Plugin.Author, container.Plugin.Description));
                        }
                    }
                }


                catch (Exception ex)
                {

                    logger.LogError(string.Format("[CSharp] Failed to load {0}, Error: {1}", info.Name, ex));
                }

            }
        }

        private bool IsSecure(Assembly assembly, out ViolationType violationType)
        {
            if (Redox.config.PluginSecurity)
            {
                if (assembly.IsFullyTrusted)
                {
                    bool flag = assembly.GetManifestResourceNames().Count() == 0;

                    if (flag)
                        violationType = ViolationType.None;
                    else
                        violationType = ViolationType.Resources;

                    return flag;
                }
                violationType = ViolationType.UntrustedAssembly;
                return false;
            }
            violationType = ViolationType.None;
            return true;
        }

        public void UnloadPlugins()
        {
            foreach (var container in PluginCollector.GetCollector().GetPlugins().Where(x => x.Running && x.Language == Language))
                UnloadPlugin(container.Plugin.Title, container);

        }
        public void UnloadPlugin(string name, PluginContainer pc = null)
        {
            var container = pc ?? PluginCollector.GetCollector().GetContainer(name);       

            if (container != null)
            {  
                if(container.Language == this.Language)
                {
                    if (container.Running)
                    {
                        container.Disable();
                        PluginCollector.GetCollector().RemovePlugin(container);
                        logger.LogInfo(string.Format("[CSharp] Succesfully unloaded plugin {0}", container.Plugin.Title));
                    }
                    else
                        logger.LogWarning($"[CSharp] Plugin \"{container.Plugin.Title}\" is already unloaded, Use /redox load instead");
                }
               
            }


        }

        public void ReloadPlugins()
        {
            UnloadPlugins();
            LoadPlugins();
        }

        public void ReloadPlugin(string Name)
        {
            var container = PluginCollector.GetCollector().GetContainer(Name);

            if (container != null)
            {
                container.Disable();
                container.Start();

                logger.LogInfo($"[CSharp] Succesfully reloaded plugin \"{container.Plugin.Title}\"");
            }
            else
                logger.LogWarning($"[CSharp] There is no plugin with the name \"{Name}\"");
        }

        private bool ValidPlugin(Assembly assembly)
        {
            return true;
            /*
            foreach (var dir in Directory.GetDirectories(path))
            {
                foreach (var file in Directory.GetFiles(dir, Expression))
                {
                    return assembly.GetReferencedAssemblies().Any(x => x.Name == new FileInfo(file).Name.Replace(".dll", string.Empty));

                }
            }
            return false;
            */
        }


    }
}