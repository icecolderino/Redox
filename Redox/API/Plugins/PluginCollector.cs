
using System;
using System.Linq;
using System.Collections.Generic;

using Redox.Core.PluginEngines;
using Redox.Core.Plugins;

namespace Redox.API.Plugins
{
    public sealed class PluginCollector
    {
        private readonly Dictionary<string, PluginContainer> Plugins;

        private static PluginCollector collector;

        public PluginCollector()
        {
            this.Plugins = new Dictionary<string, PluginContainer>();
        }

        public static PluginCollector GetCollector()
        {
            if(collector == null)
                collector = new PluginCollector();
            return collector;

        }

        /// <summary>
        /// Loads a new plugin by directory name
        /// </summary>
        /// <param name="dirName"></param>
        public void LoadPlugin(string dirName)
        {
            PluginEngines.LoadPlugin(dirName);
        }
        /// <summary>
        /// Unloads the specified plugin
        /// </summary>
        /// <param name="Title"></param>
        public void UnloadPlugin(string Title)
        {
            var container = Plugins.Values.FirstOrDefault(x => x.Plugin.Title.ToLower() == Title.ToLower());

            PluginEngines.UnloadPlugin(null, container);
        }
        /// <summary>
        /// Reloads the specified plugin
        /// </summary>
        /// <param name="Name"></param>
        public void ReloadPlugin(string Title)
        {
            var container = GetContainer(Title);
            if (container != null)
            {
                if (container.Running)
                    PluginEngines.ReloadPlugin(Title);
            }
        }
        /// <summary>
        /// Unloads all plugins
        /// </summary>
        public void UnloadAll()
        {
            PluginEngines.UnloadAllPlugins();
        }
        /// <summary>
        /// Reloads all plugins
        /// </summary>
        public void ReloadAll()
        {
            PluginEngines.ReloadAllPlugins();
        }
        /// <summary>
        /// Adds a new plugin to the collector
        /// </summary>
        /// <param name="container"></param>
        public void AddPlugin(PluginContainer container)
        {
            string name = container.Plugin.Title.ToLower();

            if (!Plugins.ContainsKey(name))
                Plugins.Add(name, container);

            container.Start();            
        }

        /// <summary>
        /// Removes the plugin from the collector
        /// </summary>
        /// <param name="container"></param>
        public void RemovePlugin(PluginContainer container)
        {
            string name = container.Plugin.Title;

            if(Plugins.ContainsKey(name))
            {
                Plugins.Remove(name);
                container.Disable();

            }
        }

        /// <summary>
        /// Returns the plugin associated with the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Plugin GetPlugin(string name)
        {
            name = name.ToLower();
            if (Plugins.ContainsKey(name))
                return Plugins[name].Plugin;
            return null;
        }

        /// <summary>
        /// Returns the plugin's container associated with the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PluginContainer GetContainer(string name)
        {
            name = name.ToLower();
            if (Plugins.ContainsKey(name))
                return Plugins[name];
            return null;
        }
        public IReadOnlyCollection<PluginContainer> GetPlugins()
        {
            List<PluginContainer> list = new List<PluginContainer>();

            foreach (var x in Plugins.Values)
                list.Add(x);
            return list.AsReadOnly();
        }

        /// <summary>
        /// Invokes a method in all plugins
        /// </summary>
        /// <param name="hookName"></param>
        /// <param name="args"></param>
        public void CallHook(string hookName, params object[] args)
        {
            foreach (var container in Plugins.Values)
            {      
                container.Plugin.Call(hookName, args);
            }

        }
    }
}
