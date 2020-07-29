using System.Collections.Generic;
using System.Linq;
using Redox.Core.Plugins;

namespace Redox.API.Plugins
{
    public sealed class PluginManager
    {
        private readonly Dictionary<string, Plugin> _plugins = new Dictionary<string, Plugin>();

        /// <summary>
        /// Loads a new plugin into the server.
        /// </summary>
        /// <param name="dir">The directory name of the plugin.</param>
        public void LoadPlugin(string dir)
        {
            Redox.Mod.EngineManager.LoadPlugin(dir);
        }

        /// <summary>
        /// Unloads a plugin.
        /// </summary>
        /// <param name="name">The title of the plugin.</param>
        public void UnloadPlugin(string name)
        {
            Redox.Mod.EngineManager.UnloadPlugin(name);
        }

        public void ReloadPlugin(string name)
        {
            Redox.Mod.EngineManager.ReloadPlugin(name);
        }

        /// <summary>
        /// Adds a new plugin to the manager.
        /// </summary>
        /// <param name="plugin">The plugin you want to add.</param>
        public void AddPlugin(Plugin plugin)
        {
            if (_plugins.ContainsKey(plugin.Info.Title))
                return;
            _plugins.Add(plugin.Info.Title, plugin);
        }

        /// <summary>
        /// Removes a plugin from the manager.
        /// </summary>
        /// <param name="title"></param>
        public void RemovePlugin(string title)
        {
            if (!_plugins.ContainsKey(title))
                return;
            _plugins.Remove(title);
        }

        /// <summary>
        /// Gets a plugin.
        /// </summary>
        /// <param name="title">The title of the plugin you want to get.</param>
        /// <returns>Returns the plugin.</returns>
        public Plugin GetPlugin(string title)
        {
            Plugin plugin = null;
            _ = _plugins.TryGetValue(title, out plugin);
            return plugin;
        }

        /// <summary>
        /// Get all plugins.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Plugin> GetPlugins()
        {
            return _plugins.Values.AsEnumerable();
        }

        /// <summary>
        /// Call a method inside all loaded plugins.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameters">Parameters</param>
        public void CallHook(string name, params object[] parameters)
        {
            foreach (var plugin in _plugins.Values)
                plugin.Call(name, parameters);
        }
    }
}