using System;
using System.Reflection;
using System.Collections.Generic;

using Redox.API;
using Redox.API.Commands;
using Redox.API.Configuration;
using Redox.API.DependencyInjection;

namespace Redox.Core.Plugins
{
    public sealed class PluginContainer : IDisposable
    {

        private readonly IDictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

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

        }
        public void Start()
        {
            this.Plugin.LoadMethods();
            this.Plugin.Commands = CommandManager.GetInstance(Plugin);
            this.Plugin.DefaultConfig = new Config(this.Plugin.Title, Plugin);
            this.Plugin.CheckTranslation();
            this.Plugin.Initialize();
            this.Running = true;
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
