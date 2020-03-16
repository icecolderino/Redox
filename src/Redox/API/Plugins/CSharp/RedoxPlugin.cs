
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

using Redox.API;
using Redox.API.DependencyInjection;
using Redox.API.Entity;
using Redox.Core.Plugins;

namespace Redox.API.Plugins.CSharp
{
    public abstract class RedoxPlugin : Plugin
    {         
        protected override PluginCollector Collector => PluginCollector.GetCollector();
        public override IServer Server => DependencyContainer.Resolve<IServer>();
        public override IEntityManager World => DependencyContainer.Resolve<IEntityManager>();

        private readonly IDictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

        protected abstract void Load();

        protected abstract void Unload();


        internal override void Initialize()
        {
            this.Load();
            if(base.LicenseURL.ToString() != "https://yourlicenseurl.com/")
            {
                Logger.Log($"[{Title}] Is licensed by: {base.LicenseURL}");
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
        public override async Task<object> CallAsync(string name, params object[] args)
        {
            return await Task.Run(() => { return Call(name, args); });
        }

        public override void LoadMethods()
        {
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (Methods.ContainsKey(method.Name)) continue;
                var attribute = method.GetCustomAttributes<IgnoreCollector>();
                if (attribute.Count() == 0)
                    Methods.Add(method.Name, method);
            
            }  
        }
    }
}
            