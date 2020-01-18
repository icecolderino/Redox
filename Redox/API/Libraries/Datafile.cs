using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace Redox.API.Libraries
{
    public sealed class Datafile
    {

        private Dictionary<string, object> _settings;

        private readonly string path;


        public Datafile(string Name)
        {
            _settings = new Dictionary<string, object>();
            path = Path.Combine(Redox.DataPath, Name + ".json");              
        }

        public void Load()
        {
            try
            {
                if (File.Exists(path))
                {
                    _settings = JSONHelper.FromFile<Dictionary<string, object>>(path);
                }
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError(string.Format("[Redox] An exception has thrown while trying to deserialize datafile {0}, Error: {1}", Path.GetFileName(path), ex.Message));
            }
          
        }
        public void Save()
        {
            JSONHelper.ToFile(path, _settings);
        }

        public object this[string key]
        {
            get
            {
                object value;

                if (_settings.TryGetValue(key, out value))
                    return value;
                return null;
            }
            set
            {
                _settings[key] = value;
            }
        }

   
        public static void WriteObject<T>(string filename, T obj)
        {
            JSONHelper.ToFile(Path.Combine(Redox.DataPath, filename), obj);
        }
        public static T ReadObject<T>(string filename)
        {
            return JSONHelper.FromFile<T>(Path.Combine(Redox.DataPath, filename));
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
            return JSONHelper.ToJson(_settings);
        }
    }
}
