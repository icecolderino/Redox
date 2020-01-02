
using System;
using System.Collections.Generic;

using Redox.Core.Plugins;

namespace Redox.API.Libraries
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


        public void AddPlugin(PluginContainer container)
        {
            string name = container.Plugin.Title;

            if (!Plugins.ContainsKey(name))
                Plugins.Add(name, container);

            container.Start();
        }

        /// <summary>
        /// Returns the container associated with the plugin
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Plugin GetPlugin(string name)
        {
            if (Plugins.ContainsKey(name))
                return Plugins[name].Plugin;
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
            foreach(var container in Plugins.Values)
            {
                container.Plugin.Call(hookName, args);
            }
        }
    }
}
