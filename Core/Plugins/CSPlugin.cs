﻿using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Redox.Core.Plugins
{
    /// <summary>
    /// Generic Base representation of a CSharp Plugin
    /// </summary>
    public abstract class CSPlugin : Plugin
    {
        private readonly Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

     
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
        public T Call<T>(string name, object[] parameters)
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
                if (Methods.ContainsKey(method.Name)) 
                    continue;
                Methods.Add(method.Name, method);

            }
        }
    }
}
