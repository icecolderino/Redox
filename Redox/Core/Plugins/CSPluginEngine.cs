using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Security;
using System.Security.Permissions;

using Redox.API;
using Redox.API.Libraries;
using Redox.API.Plugins;
using Redox.API.DependencyInjection;
using Redox.Core.PluginEngines;

namespace Redox.Core.Plugins
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

            PluginCollector.GetCollector().CallHook("OnPluginsLoaded");
            logger.LogInfo($"[CSharp] Loaded {PluginCollector.GetCollector().GetPlugins().Count} plugins.");
          
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
                                
                                PluginContainer container = new PluginContainer(plugin, instance, Language);
                                container.Plugin.PluginPath = info.DirectoryName + "\\";
                                PluginCollector.GetCollector().AddPlugin(container);

                                logger.LogInfo(string.Format("[CSharp] Succesfully loaded plugin {0}, {1}, Author {2} ({3}", plugin.Title, plugin.Version, plugin.Author, plugin.Description));
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
                catch (Exception ex)
                {
                    
                    logger.LogError(string.Format("[CSharp] Failed to load {0}, Error: {1}", info.Name, ex));
                }
                
            }         
        }

        private bool IsSecure(Assembly assembly, out ViolationType violationType)
        {
            if(Redox.config.PluginSecurity)
            {
                if(assembly.IsFullyTrusted)
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

            if(container != null)
            {
                PluginCollector.GetCollector().RemovePlugin(container);
                logger.LogInfo(string.Format("[CSharp] Succesfully unloaded plugin {0}", container.Plugin.Title));
            }
                

        }

        public void ReloadPlugins()
        {
            UnloadPlugins();
            LoadPlugins();
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