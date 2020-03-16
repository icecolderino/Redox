using System;
using System.Reflection;
using System.Collections.Generic;

using Redox.Core.Plugins;

using Redox.API;
using Redox.API.Commands;
using Redox.API.Configuration;
using Redox.API.Plugins.Watcher;

namespace Redox.API.Plugins
{
    public sealed class PluginContainer : IDisposable
    {

        public bool Running
        {
            get;
            private set;
        }    
        
        public Plugin Plugin
        {
            get;
            private set;
        }

        public PluginWatcher Watcher
        {
            get;
            private set;
        }
        
        public string Language
        {
            get;
            private set;
        }

        public PluginContainer(Plugin plugin, object instance, string Language)
        {
            this.Running = true;
            this.Plugin = plugin;
            this.Language = Language;
        //    this.Watcher = new PluginWatcher(Plugin);
        }
        public void Start()
        {
            this.Plugin.LoadMethods();
            this.Plugin.Commands = CommandManager.GetInstance(Plugin);
            this.Plugin.DefaultConfig = new Config("Configuration", Plugin);
            this.Plugin.CheckTranslation();
            this.Plugin.Initialize();
            this.Running = true;
           // PluginCollector.GetCollector().CallHook("OnPluginLoaded", Plugin);
        }

        public void Disable()
        {
            this.Plugin.Deinitialize();
            this.Running = false;
        }


        public void Dispose()
        {
            this.Plugin.Dispose();
        }

    }
}
