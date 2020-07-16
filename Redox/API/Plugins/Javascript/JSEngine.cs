using System;
using System.IO;
using System.Collections.Generic;

using Redox.Core.PluginEngines;
using System.Linq;

namespace Redox.API.Plugins.Javascript
{
    public sealed class JSEngine : IPluginEngine
    {

        private readonly Dictionary<string, JSPlugin> Plugins = new Dictionary<string, JSPlugin>();
        public string Language => "Javascript";

        public string Pattern => "*.js";

        public void LoadPlugins()
        {
            foreach (string dir in Directory.GetDirectories(Bootstrap.RedoxMod.PluginPath))
                LoadPlugin(dir);
        }

        public void LoadPlugin(string dir)
        {
            foreach (string file in Directory.GetFiles(dir, Pattern))
            {
                try
                {
                    FileInfo info = new FileInfo(file);
                    string name = Path.GetFileNameWithoutExtension(info.Name);
                    if (!Plugins.ContainsKey(name))
                    {
                        JSPlugin plugin = new JSPlugin(File.ReadAllText(file));
                        if(name != plugin.Title)
                        {
                            Bootstrap.RedoxMod.Logger.LogWarning($"[Jint] Failed to load plugin {plugin.Title} because the file name is not the same as the title");
                            return;
                        }
                        plugin.FileInfo = info;
                        PluginContainer container = new PluginContainer(plugin, null, Language);
                        PluginCollector.GetCollector().AddPlugin(container);
                        Plugins.Add(plugin.Title, plugin);
                        Bootstrap.RedoxMod.Logger.LogInfo(string.Format("[Jint] Succesfully loaded plugin {0}, Version: {1}, Author {2} ({3})", plugin.Title, plugin.Version, plugin.Author, plugin.Description));
                    }
                }
                catch(Exception ex)
                {
                    Bootstrap.RedoxMod.Logger.LogError(string.Format("[Jint] Failed to load {0}, Error: {1}", Path.GetFileNameWithoutExtension(file), ex));
                }
               
            }
        }


        public void ReloadPlugin(string name)
        {
            if(Plugins.TryGetValue(name, out JSPlugin plugin))
            {
                UnloadPlugin(name);
                LoadPlugin(plugin.FileInfo.DirectoryName);
            }
        }

        public void ReloadPlugins()
        {
            foreach(var plugin in Plugins.Values.ToList())
            {
                UnloadPlugin(plugin.Title);
                LoadPlugin(plugin.Title);
            }
        }

        public void UnloadPlugin(string name, PluginContainer pc = null)
        {
            if (Plugins.TryGetValue(name, out JSPlugin plugin))
            {
                PluginCollector.GetCollector().RemovePlugin(PluginCollector.GetCollector().GetContainer(name));
                Plugins.Remove(name);
                Bootstrap.RedoxMod.Logger.LogInfo("[Jint] Succesfully unloaded plugin " + name);
            }
        }     
        public void UnloadPlugins()
        {
            foreach(var plugin in Plugins.Values.ToList())
            {
                UnloadPlugin(plugin.Title);
            }
        }
    }
}
