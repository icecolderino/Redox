using System;
using System.IO;
using System.Collections.Generic;

using Redox.Core.PluginEngines;

namespace Redox.API.Plugins.Lua
{
    public sealed class LuaEngine : IPluginEngine
    {
        public string Language => "Lua";

        public string Pattern => "*.lua";

        private readonly Dictionary<string, LuaPlugin> Plugins = new Dictionary<string, LuaPlugin>();

        public void LoadPlugins()
        {
            foreach(string dir in Directory.GetDirectories(Redox.PluginPath))
            {
                LoadPlugin(dir);
            }
        }
        public void LoadPlugin(string dir)
        {
            foreach (string file in Directory.GetFiles(dir, Pattern))
            {
                FileInfo info = new FileInfo(file);
                string name = info.Name.Replace(".lua", string.Empty);
                try
                {                 
                    if (!Plugins.ContainsKey(name))
                    {
                        LuaPlugin plugin = new LuaPlugin(info, File.ReadAllText(file));
                        PluginContainer container = new PluginContainer(plugin, null, Language);
                        container.Plugin.FileInfo = info;
                        PluginCollector.GetCollector().AddPlugin(container);
                        Redox.Logger.LogInfo(string.Format("[Lua] Succesfully loaded plugin {0}, {1}, Author {2} ({3})", plugin.Title, plugin.Version, plugin.Author, plugin.Description));

                    }
                }
                catch(Exception ex)
                {
                    Redox.Logger.LogError(string.Format("[Redox] Failed to load plugin {0} error: {1}", name, ex));
                }
             
            }
                
        }

        public void ReloadPlugin(string Name)
        {
            throw new NotImplementedException();
        }

        public void ReloadPlugins()
        {
            throw new NotImplementedException();
        }

        public void UnloadPlugin(string Name, PluginContainer container = null)
        {
            throw new NotImplementedException();
        }

        public void UnloadPlugins()
        {
            throw new NotImplementedException();
        }
    }
}
