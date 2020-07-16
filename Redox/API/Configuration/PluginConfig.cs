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
    public class PluginConfig : IConfiguration
    {
             
        private readonly Plugin plugin;
        private Dictionary<string, object> Settings;
        private readonly string name;

        public bool Exists
        {
            get
            {
                return File.Exists(Path.Combine(plugin.FileInfo.DirectoryName, name));
            }
        }

        public PluginConfig(string name, Plugin plugin)
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
        public void AddSetting(string key, object value)
        {
           
            if (!Settings.ContainsKey(key))
            {
                Settings.Add(key, value);
            }
                
                
        }

        public void SetSetting(string key, object value)
        {
           // this[key] = value;
            
            if (Settings.ContainsKey(key))
                Settings[key] = value;
            else
                AddSetting(key, value);
                
        }
        public object GetSetting(string key)
        {
          //  return this[key];
            
            if (Settings.ContainsKey(key))
                return Settings[key];
            return null;
           
        }  
        public bool TryGetSetting(string key, out object ob)
        {
            if(Settings.ContainsKey(key))
            {
                ob = Settings[key];
                return true;
            }
            ob = null;
            return false;
        }

        public bool HasSetting(string key)
        {
            return Settings.ContainsKey(key);
        }
        public Dictionary<string, object> GetSettings()
        {
            return Settings;
        }
        public void Load()
        {
            if (Exists)
            {
                string path = Path.Combine(plugin.FileInfo.DirectoryName, name);
                Settings = Utility.Json.FromFile<Dictionary<string, object>>(path);              
            }
        }
        public void Save()
        {

            string path = Path.Combine(plugin.FileInfo.DirectoryName, name);
            Utility.Json.ToFile(path, Settings);       
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
        public async Task LoadAsync()
        {
            await Task.Run(() => Load());
        }

        public async Task SaveAsync()
        {
            await Task.Run(() => Save());
        }
    }
    
}
  