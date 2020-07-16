using System;
using System.IO;
using System.Collections.Generic;
using Redox.API.Serialization;

namespace Redox.API.Data
{
    public sealed class Datafile
    {

        private Map<string, object> _settings;

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
            _settings = new Map<string, object>();
            path = Path.Combine(Bootstrap.RedoxMod.DataPath, Name + ".json");              
        }

        public void Load()
        {
            try
            {
                if (File.Exists(path))
                {
                    _settings = Utility.Json.FromFile<Map<string, object>>(path);
                }
            }
            catch(Exception ex)
            {
                Bootstrap.RedoxMod.Logger.LogError(string.Format("[Bootstrap.RedoxMod] An exception has thrown while trying to deserialize datafile {0}, Error: {1}", Path.GetFileName(path), ex.Message));
            }
          
        }
        public void Save()
        {
            Utility.Json.ToFile(path, _settings);
        }

        public object this[string key]
        {
            get
            {

                if (_settings.TryGetValue(key, out object value))
                    return value;
                return null;
            }
            set
            {
                if (_settings.ContainsKey(key))
                    _settings[key] = value;
                else
                    _settings.Add(key, value);
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
            _settings.Remove(key);
        }
        public void Clear()
        {
            _settings.Clear();
        }
     
        public override string ToString()
        {
            return Utility.Json.ToJson(_settings);
        }
    }
}
