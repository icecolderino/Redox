using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using Redox.Core.PluginEngines;
using Redox.API.Libraries;

namespace Redox.API.Plugins.CSharp
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
        private static readonly Dictionary<string, RedoxPlugin> Plugins = new Dictionary<string, RedoxPlugin>();
        private static readonly Dictionary<string, Assembly> Assemblies = new Dictionary<string, Assembly>();
        private static readonly string path = Redox.PluginPath;

        public string Language => "CSharp";
        public string Pattern => "*.dll";

        public Dictionary<string, object> Values => new Dictionary<string, object>();

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
                    string name = Path.GetFileNameWithoutExtension(info.Name);
                    if (!Assemblies.TryGetValue(name, out Assembly assembly))
                    {
                        assembly = Assembly.Load(File.ReadAllBytes(file));
                        Assemblies.Add(name, assembly);
                    }
                    if (Plugins.ContainsKey(name))
                        continue;

                    if (this.IsSecure(assembly, out ViolationType violationType))
                    {
                        foreach (Type type in assembly.GetExportedTypes())
                        {
                            if (type.IsSubclassOf(typeof(RedoxPlugin)) && type.IsPublic && !type.IsAbstract)
                            {
                                object instance = Activator.CreateInstance(type);
                                RedoxPlugin plugin = (RedoxPlugin)instance;

                                if (((plugin.CoreVersion.ToString() == "0.0.0.0") || (plugin.CoreVersion >= Redox.version)) || Redox.config.LoadIncompitablePlugins)
                                {
                                    plugin.FileInfo = info;
                                    PluginContainer container = new PluginContainer(plugin, instance, Language);
                                    PluginCollector.GetCollector().AddPlugin(container);
                                    Plugins.Add(name, plugin);
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
            foreach(var plugin in Plugins.Values)
            {
                UnloadPlugin(plugin.Title);
            }
        }
        public void UnloadPlugin(string name, PluginContainer pc = null)
        {
            if(Plugins.TryGetValue(name, out RedoxPlugin plugin))
            {
                PluginCollector.GetCollector().GetContainer(name).Disable();
                Plugins.Remove(name);
                logger.LogInfo("[CSharp] Succesfully unloaded plugin " + plugin.Title);
            }
        }

        public void ReloadPlugins()
        {
            UnloadPlugins();
            LoadPlugins();
        }

        public void ReloadPlugin(string Name)
        {
            if(Plugins.TryGetValue(Name, out RedoxPlugin plugin))
            {
                plugin.Deinitialize();
                plugin.Initialize();
                logger.LogInfo("[CSharp] Succesfully reloaded plugin " + Name);
            }
        }
    }
}