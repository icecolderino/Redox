using System;
using System.IO;
using System.Collections.Generic;

using Redox.Core.Plugins;

namespace Redox.API.Plugins.Watcher
{
    public sealed class PluginWatcher
    {
        public static IDictionary<Plugin, PluginWatcher> Watchers = new Dictionary<Plugin, PluginWatcher>();

        private readonly Plugin Plugin;
        private readonly FileSystemWatcher Watcher;
        
        public PluginWatcher(Plugin plugin)
        {
            Plugin = plugin;
            Watcher = new FileSystemWatcher(plugin.FileInfo.FullName);

            Watcher.Changed += this.OnChanged;
            Watcher.Deleted += this.OnDeleted;

            Watcher.EnableRaisingEvents = true;

            Watchers.Add(Plugin, this);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            PluginCollector.GetCollector().UnloadPlugin(Plugin.Title);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            PluginCollector.GetCollector().ReloadPlugin(Plugin.Title);
        }
    }
}
