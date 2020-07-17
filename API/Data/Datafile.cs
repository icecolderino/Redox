using System;
using System.IO;
using System.Collections.Generic;

namespace Redox.API.Data
{
    public sealed class Datafile
    {

        private IDictionary<string, object> settings;

        private readonly string path;

        public bool Exists
        {
            get
            {
                return File.Exists(path);
            }
        }

        public Datafile(string Name)
        {
            settings = new Dictionary<string, object>();
            path = Path.Combine(Bootstrap.RedoxMod.DataPath, Name + ".json");              
        }

        public void Load()
        {
            try
            {
                if (File.Exists(path))
                {
                    settings = Utility.Json.FromFile<Dictionary<string, object>>(path);
                }
            }
            catch(Exception ex)
            {
                Bootstrap.RedoxMod.Logger.LogError(string.Format("[Bootstrap.RedoxMod] An exception has thrown while trying to deserialize datafile {0}, Error: {1}", Path.GetFileName(path), ex.Message));
            }
          
        }
        public void Save()
        {
            Utility.Json.ToFile(path, settings);
        }

        public object this[string key]
        {
            get
            {

                if (settings.TryGetValue(key, out object value))
                    return value;
                return null;
            }
            set
            {
                if (settings.ContainsKey(key))
                    settings[key] = value;
                else
                    settings.Add(key, value);
            }
        }

   
        public void WriteObject(object obj)
        {
            Utility.Json.ToFile(path, obj);
        }
        public T ReadObject<T>()
        {
            return Utility.Json.FromFile<T>(path);
        }
        public void Remove(string key)
        {
            settings.Remove(key);
        }
        public void Clear()
        {
            settings.Clear();
        }
     
        public override string ToString()
        {
            return Utility.Json.ToJson(settings);
        }
    }
}
