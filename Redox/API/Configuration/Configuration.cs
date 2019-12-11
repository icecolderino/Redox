using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


using Newtonsoft.Json;
using Redox.Core.Plugin;

namespace Redox.API.Configuration
{
    /// <summary>
    /// Represents a json configuration
    /// </summary>
    public class Configuration
    {
        
        private readonly RedoxPlugin plugin;
        private Dictionary<string, object> Settings;
        private string name;

        public Configuration(string name,RedoxPlugin plugin)
        {
            this.name = name + ".json";
            Settings = new Dictionary<string, object>();
            this.plugin = plugin;
        }

        public void AddSetting(string key, object value)
        {
            if (!Settings.ContainsKey(key))
                Settings.Add(key, value);
        }
        public object GetSetting(string key)
        {
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

        public bool Exists()
        {
            return File.Exists(Path.Combine(plugin.Path, name));
        }
        public void LoadConfig()
        {
            if(Exists())
            {
                Settings.Clear();
                Settings = JsonConvert.DeserializeObject(File.ReadAllText(Path.Combine(plugin.Path, name))) as Dictionary<string, object>;
            }
        }
        public void Save()
        {
            StreamWriter writer = new StreamWriter(Path.Combine(plugin.Path, name));
            writer.Write(JsonConvert.SerializeObject(Settings, Formatting.Indented));
            writer.Close();

        }
    }
}
  