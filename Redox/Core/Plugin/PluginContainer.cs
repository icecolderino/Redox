using System;
using System.Reflection;
using System.Collections.Generic;

namespace Redox.Core.Plugin
{
    public sealed class PluginContainer : IDisposable
    {

        public readonly IDictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

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
            this.Plugin.Start();
            this.Running = true;
        }
        public void Disable()
        {
            this.Plugin.Disable();
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
                return Methods[name].Invoke(name, parameters);
            return null;
        }





















        public void Dispose()
        {
            this.Plugin.Dispose();
        }




    }
}
