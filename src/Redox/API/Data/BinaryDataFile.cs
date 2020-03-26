using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Redox.API.Serialization;

namespace Redox.API.Data
{
    public sealed class BinaryDatafile
    {
       

        private readonly string path;

        private Map<string, object> _settings;

        public bool Exists
        {
            get
            {
                return File.Exists(path);
            }
        }
        public BinaryDatafile(string Name)
        {
            _settings = new Map<string, object>();
            path = Path.Combine(Redox.DataPath, Name + ".bin");
        }



        public void Load()
        {
            try
            {
                if (Exists)
                {
                    using(FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        _settings = formatter.Deserialize(fs) as Map<string, object>;
                    } 
                }
            }
            catch (Exception ex)
            {
                Redox.Logger.LogError(string.Format("[Redox] An exception has thrown while trying to deserialize datafile {0}, Error: {1}", Path.GetFileName(path), ex.Message));
            }

        }
        public void Save()
        {
            using(FileStream fs  = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, _settings);
            }
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
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }
        public T ReadObject<T>()
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }
        public void Remove(string key)
        {
            _settings.Remove(key);
        }
        public void Clear()
        {
            _settings.Clear();
        }
    }
}
