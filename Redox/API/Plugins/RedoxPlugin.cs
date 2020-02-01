
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Redox.API;
using Redox.API.Libraries;
using Redox.API.DependencyInjection;
using Redox.API.Entity;
using Redox.Core.Plugins;

namespace Redox.API.Plugins
{
    public abstract class RedoxPlugin : Plugin
    {         
        protected override PluginCollector Collector => PluginCollector.GetCollector();
        protected override IServer Server => DependencyContainer.Resolve<IServer>();
        protected override IEntityManager World => DependencyContainer.Resolve<IEntityManager>();

        private readonly IDictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

        protected abstract void Load();

        protected abstract void Unload();

        internal override void Initialize()
        {
            this.Load();
            if(base.LicenseURL.ToString() != "https://yourlicenseurl.com/")
            {
                Logger.LogColor($"[{Title}] Is licensed by: {base.LicenseURL}", ConsoleColor.DarkYellow);
            }
        }
        internal override void Deinitialize()
        {
            this.Unload();
        }

        /// <summary>
        /// Invokes a method in the plugin
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override object Call(string name, object[] parameters)
        {
            if (Methods.ContainsKey(name))
                return Methods[name].Invoke(this, parameters);
            return null;
        }
        public override T Call<T>(string name, object[] parameters)
        {
            return (T)Call(name, parameters);
        }
        public override void LoadMethods()
        {
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (Methods.ContainsKey(method.Name)) return;
                object[] attributes = method.GetCustomAttributes(typeof(IgnoreCollector), true);
                if (attributes.Length == 0)
                    Methods.Add(method.Name, method);
            
            }  
        }
    }
}
            