using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace Redox.API.Libraries
{
    public class Datafile : IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {

        private Dictionary<string, object> _settings;

        private readonly string path;


        public Datafile(string Name) : base()
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

   
        public void WriteObject<T>(T obj)
        {
            JSONHelper.ToFile(path, obj);
        }
        public T ReadObject<T>()
        {
            return JSONHelper.FromFile<T>(path);
        }


        public void Remove(string key)
        {
            _settings.Remove(key);
        }
        public void Clear()
        {
            _settings.Clear();
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _settings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _settings.GetEnumerator();
        }

        public override string ToString()
        {
            return JSONHelper.ToJson(_settings);
        }
    }
}
