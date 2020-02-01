﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Redox.Core.Plugins;
using Redox.Core.Configuration;

using Redox.API.Helpers;

namespace Redox.API.Configuration
{

    public enum ConfigType : ushort
    {
        JSON = 0,
        YAML = 1
    }
    /// <summary>
    /// Represents a json & YAML configuration
    /// </summary>
    public class Config : IConfiguration
    {
             
        private readonly Plugin plugin;
        private Dictionary<string, object> Settings;
        private string name;
        private ConfigType configType;

        public Config(string name, Plugin plugin, ConfigType configType = ConfigType.JSON)
        {
            Settings = new Dictionary<string, object>();
            this.plugin = plugin;
            this.configType = configType;
            this.name = configType == ConfigType.JSON ? name + ".json" : name + ".yml";
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
            return File.Exists(Path.Combine(plugin.FileInfo.DirectoryName, name));
        }
        public void Load()
        {
            if (Exists())
            {
                string path = Path.Combine(plugin.FileInfo.DirectoryName, name);
                if (configType == ConfigType.JSON)
                    JSONHelper.FromFile<Dictionary<string, object>>(path);
                else
                    YAMLHelper.FromFile<Dictionary<string, object>>(path);
            }
        }
        public void Save()
        {

            string path = Path.Combine(plugin.FileInfo.DirectoryName, name);

            if (configType == ConfigType.JSON)
                JSONHelper.ToFile(path, Settings);
            else
                YAMLHelper.ToFile(path, Settings);
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
  