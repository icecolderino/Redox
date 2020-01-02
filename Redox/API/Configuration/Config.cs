using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Redox.Core.Configuration;
using Redox.Core.Plugins;

namespace Redox.API.Configuration
{
    /// <summary>
    /// Represents a json & XML configuration
    /// </summary>
    public class Config : IConfiguration
    {
       
        
        private readonly Plugin plugin;
        private Dictionary<string, object> Settings;
        private string name;

        public Config(string name, Plugin plugin)
        {          
            Settings = new Dictionary<string, object>();
            this.plugin = plugin;
            this.name = name + ".json";
        }

        public void AddSetting(string key, object value)
        {
            if (!Settings.ContainsKey(key))
                Settings.Add(key, value);
            else
                SetSetting(key, value);
        }

        public void SetSetting(string key, object value)
        {
            if (Settings.ContainsKey(key))
                Settings[key] = value;
            else
                AddSetting(key, value);
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
        public async Task LoadConfig()
        {
            await Task.Run(() =>
            {
                if (Exists())
                {
                    var text = File.ReadAllText(Path.Combine(plugin.Path, name));
                    Settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                }
            });                 
        }
        public async Task Save()
        {
            await Task.Run(() =>
            {
                try
                {
                    StreamWriter writer = new StreamWriter(Path.Combine(plugin.Path, name));
                    writer.Write(JsonConvert.SerializeObject(Settings, Newtonsoft.Json.Formatting.Indented));
                    writer.Close();
                }
                catch (Exception ex)
                {

                }
            });

        }
    }
}
  