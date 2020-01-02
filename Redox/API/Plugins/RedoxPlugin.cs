using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Redox.API.Configuration.Translation;
using Redox.API.Libraries;
using Redox.API.DependencyInjection;
using Redox.Core.Plugins;
using Redox.API.Entity;

namespace Redox.API.Plugins
{
    public abstract class RedoxPlugin : Plugin
    {         
        protected override PluginCollector Manager => PluginCollector.GetCollector();
        protected override IServer Server => DependencyContainer.Resolve<IServer>();
        protected override IEntityManager World => DependencyContainer.Resolve<IEntityManager>();

        private readonly IDictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

        public abstract void Load();

        public abstract void Unload();



        public override void Initialize()
        {
            this.Load();
        }
        public override void Deinitialize()
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
            Logger = DependencyContainer.Resolve<ILogger>();
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                object[] attributes = method.GetCustomAttributes(typeof(NotCollectable), true);
                if (attributes.Length == 0)
                {
                    Methods.Add(method.Name, method);
                    Logger.Log("[CSharp]Loaded method: " + method.Name);
                }
            
            }  
        }
    }
}
            