using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;


using Newtonsoft.Json;
using Redox.API.Plugins.CSharp;
using Redox.Core.Configuration;
using Redox.Core.Plugins;
using System.Threading.Tasks;

namespace Redox.API.Configuration
{

    
    /// <summary>
    /// Represents a json configuration
    /// </summary>
    public class Configuration : IConfiguration
    {
             
        private readonly CSPlugin plugin;
        private Dictionary<string, object> Settings;
        private readonly string name;

        public bool Exists
        {
            get
            {
                return File.Exists(Path.Combine(plugin.FileInfo.DirectoryName, name));
            }
        }

        public Configuration(string name, CSPlugin plugin)
        {
            Settings = new Dictionary<string, object>();
            this.plugin = plugin;
            this.name = name + ".json";
        }

        public object this[string key]
        {
            get
            {
                if (Settings.ContainsKey(key))
                    return Settings[key];
                return null;
            }   
        }
        public Task AddSetting(string key, object value)
        {
           
            if (!Settings.ContainsKey(key))
            {
                Settings.Add(key, value);
            }
            return Task.CompletedTask;   
                
        }

        public Task SetSetting(string key, object value)
        {
           // this[key] = value;
            
            if (Settings.ContainsKey(key))
                Settings[key] = value;
            else
                AddSetting(key, value);

            return Task.CompletedTask;
        }
        public Task<object> GetSetting(string key)
        {
          //  return this[key];
            
            if (Settings.ContainsKey(key))
                return Task.FromResult(Settings[key]);
            return null;
           
        }  
        public Task<bool> TryGetSetting(string key, out object ob)
        {
            if(Settings.ContainsKey(key))
            {
                ob = Settings[key];
                return Task.FromResult(true);
            }
            ob = null;
            return Task.FromResult(false);
        }

        public Task<bool> HasSetting(string key)
        {
            return Task.FromResult(Settings.ContainsKey(key));
        }
        public Dictionary<string, object> GetSettings()
        {
            return Settings;
        }
        public async Task LoadAsync()
        {
            string path = Path.Combine(plugin.FileInfo.DirectoryName, name);
            Settings = await Utility.Json.FromFileAsync<Dictionary<string, object>>(path);              
        }
        public async Task SaveAsync()
        {

            string path = Path.Combine(plugin.FileInfo.DirectoryName, name);
            await Utility.Json.ToFileAsync(path, Settings);       
        }

        public void Write(object obj)
        {
            Utility.Json.ToFile(Path.Combine(plugin.FileInfo.DirectoryName, name), obj);
        }
        public T Read<T>()
        {
            string path = Path.Combine(plugin.FileInfo.DirectoryName, name);
            if(Exists)
            {
                return Utility.Json.FromFile<T>(path);
            }
            return default(T);
        }
    }
    
}
  