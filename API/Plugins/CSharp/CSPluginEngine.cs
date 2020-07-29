using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using Redox.API.Libraries;
using Redox.Core.Plugins;

namespace Redox.API.Plugins.CSharp
{
    public sealed class CSPluginEngine : IPluginEngine
    {

        private readonly Dictionary<string, CSPlugin> Plugins = new Dictionary<string, CSPlugin>();
        private readonly Dictionary<string, Assembly> Assemblies = new Dictionary<string, Assembly>();
        private readonly string Path = Bootstrap.RedoxMod.PluginPath;

        public string Language => "CSharp";
        public string Pattern => "*.dll";

        private ILogger Logger => Bootstrap.RedoxMod.Logger;

        public CSPluginEngine()
        {
        }

        public void LoadPlugins()
        {
            Logger.LogInfo("[CSharp] Loading plugins..");

            foreach (var dir in Directory.GetDirectories(Path))
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
                    string name = System.IO.Path.GetFileNameWithoutExtension(info.Name);
                    if (Plugins.TryGetValue(name, out CSPlugin p))                    
                    {
                        if (p.Running)
                        {
                            Logger.LogWarning(string.Format("[CSharp] Failed to load {0} because its already initialized", p.Info.Title));
                            break;
                        }
                        p.Initialize();
                        Logger.LogInfo(string.Format("[CSharp] Succesfully loaded plugin {0}, {1}, Author {2} ({3})", p.Info.Title, p.Info.Version, p.Info.Authors, p.Info.Description));                     
                        break;
                    }
                    if (!Assemblies.TryGetValue(name, out Assembly assembly))
                    {
                        assembly = Assembly.Load(File.ReadAllBytes(file));
                        Assemblies.Add(name, assembly);
                    }
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        if (type.IsSubclassOf(typeof(CSPlugin)) && type.IsPublic && !type.IsAbstract)
                        {
                            object instance = Activator.CreateInstance(type);
                            CSPlugin plugin = (CSPlugin)instance;

                            if (name != plugin.Info.Title)
                            {
                                Logger.LogError($"[CSharp] Failed to load plugin {plugin.Info.Title} because the file name is not the same as the title");
                                return;
                            }
                            plugin.FileInfo = info;
                            PluginContainer container = new PluginContainer(plugin, instance, Language);
                            Redox.Mod.Plugins.AddPlugin(plugin);
                            Plugins.Add(plugin.Info.Title, plugin);
                            plugin.Initialize();
                            Logger.LogInfo(string.Format("[CSharp] Succesfully loaded plugin {0}, {1}, Author {2} ({3})", plugin.Info.Title, plugin.Info.Version, plugin.Info.Authors, plugin.Info.Description));
                        }
                    }
                    sw.Stop();
                    int time = sw.Elapsed.Milliseconds;

                    if (time > 500)
                        Logger.LogSpeed(string.Format("[CSharp] Plugin {0} took {1} milliseconds to load", name, time));
                }


                catch (Exception ex)
                {

                    Logger.LogError(string.Format("[CSharp] Failed to load {0}, Error: {1}", info.Name, ex));
                }

            }
        }

        public void UnloadPlugins()
        {
            foreach(var plugin in Plugins.Values)
            {
                UnloadPlugin(plugin.Info.Title);
            }
        }
        public void UnloadPlugin(string name, PluginContainer pc = null)
        {
            if(Plugins.TryGetValue(name, out CSPlugin plugin))
            {
                plugin.Deinitialize();
                Plugins.Remove(name);
                Logger.LogInfo("[CSharp] Succesfully unloaded plugin " + plugin.Info.Title);
            }
        }

        public void ReloadPlugins()
        {
            UnloadPlugins();
            LoadPlugins();
        }

        public void ReloadPlugin(string name)
        {
            if(Plugins.TryGetValue(name, out CSPlugin plugin))
            {
                plugin.Deinitialize();
                plugin.Initialize();
                Logger.LogInfo("[CSharp] Succesfully reloaded plugin " + name);
            }
        }
    }
}