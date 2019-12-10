
using System;
using System.Collections.Generic;

using Redox.Core.Plugin;

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

        /// <summary>
        /// Returns the container associated with the plugin
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PluginContainer GetPlugin(string name)
        {
            if (Plugins.ContainsKey(name))
                return Plugins[name];
            return null;
        }



    }
}
