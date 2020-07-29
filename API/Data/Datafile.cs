using System;
using System.IO;
using System.Collections.Generic;

namespace Redox.API.Data
{
    public sealed class Datafile
    {

        private IDictionary<string, object> _settings;

        private readonly string _path;

        public bool Exists
        {
            get
            {
                return File.Exists(_path);
            }
        }

        public Datafile(string name)
        {
            _settings = new Dictionary<string, object>();
            _path = Path.Combine(Bootstrap.RedoxMod.DataPath, name + ".json");              
        }

        public void Load()
        {
            try
            {
                if (File.Exists(_path))
                {
                    _settings = Utility.Json.FromFile<Dictionary<string, object>>(_path);
                }
            }
            catch(Exception ex)
            {
                Bootstrap.RedoxMod.Logger.LogError(string.Format("[RedoxMod] An exception has thrown while trying to deserialize datafile {0}, Error: {1}", Path.GetFileName(_path), ex.Message));
            }
          
        }
        public void Save()
        {
            Utility.Json.ToFile(_path, _settings);
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
            Utility.Json.ToFile(_path, obj);
        }
        public T ReadObject<T>()
        {
            return Utility.Json.FromFile<T>(_path);
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
