using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using Redox.Core.Plugin;

namespace Redox.API.Configuration
{
    /// <summary>
    /// Represents a binaire datafile
    /// </summary>
    public class Datafile
    {

        private readonly RedoxPlugin plugin;
        private Dictionary<string, object> Settings;
        private string name;

        public Datafile(string name, RedoxPlugin plugin)
        {
            this.name = name + ".data";
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
            if (Settings.ContainsKey(key))
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
            if (Exists())
            {
                Settings.Clear();

                string path = Path.Combine(plugin.Path, name);

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Settings = formatter.Deserialize(fs) as Dictionary<string, object>;
                    fs.Dispose();
                }

            }
        }
        public void Save()
        {
            string path = Path.Combine(plugin.Path, name);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, Settings);
                fs.Dispose();
            }

        }
    }
}
