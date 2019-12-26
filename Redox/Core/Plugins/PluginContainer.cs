using System;
using System.Reflection;
using System.Collections.Generic;

using Redox.API.Plugins;
using Redox.API.Commands;
using Redox.API.Configuration;

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


        public RedoxPlugin Plugin
        {
            get;
            private set;
        }
        
        public PluginContainer(RedoxPlugin plugin)
        {
            this.Running = false;
            this.Plugin = plugin;

        }
        public void Start()
        {
            this.LoadMethods();
            this.Plugin.Commands = CommandManager.GetInstance(Plugin);
            this.Plugin.DefaultConfig = new Config(this.Plugin.Title, Plugin);
            this.Plugin.Load();
            this.Running = true;
        }

        public void Disable()
        {
            this.Plugin.Unload();
            this.Running = false;
        }

        /// <summary>
        /// Invokes a method and returns a value if necessary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Call(string name, object[] parameters)
        {
            if (Methods.ContainsKey(name))
                return Methods[name].Invoke(Plugin, parameters);
            return null;
        }
        public T Call<T>(string name, object[] parameters)
        {
            return (T)Call(name, parameters);
        }

        private void LoadMethods()
        {
            foreach (var method in Plugin.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Methods.Add(method.Name, method);
            }
        }

        public void Dispose()
        {
            this.Plugin.Dispose();
        }




    }
}
