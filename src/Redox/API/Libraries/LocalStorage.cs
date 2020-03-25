using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Redox.API.Helpers;
using Redox.API.Serialization;

namespace Redox.API.Libraries
{

    /// <summary>
    /// LocalStorage for redox
    /// </summary>
    public static class LocalStorage
    {
        private static readonly string path = Path.Combine(Redox.RootPath, "Storage.data");
        private static Map<string, Map<object, object>> map = new Map<string, Map<object, object>>();


        public static void Add(string tablename, object key, object value)
        {
            if(!map.ContainsKey(tablename))
            {
                map.Add(tablename, new Map<object, object> { { key, value } });
            }
            else if(!map[tablename].ContainsKey(key))
            {
                map[tablename].Add(key, value);
            }
        }



        public static void Set(string tablename, object key, object value)
        {
            if(map.ContainsKey(tablename))
            {
                if(map[tablename].ContainsKey(key))
                {
                    map[tablename][key] = value;
                }
            }
        }
        public static void Remove(string tablename, object key)
        {
            if(map.ContainsKey(tablename))
            {
                if (map[tablename].ContainsKey(key))
                    map[tablename].Remove(key);
            }
        }
        public static bool HasKey(string tablename, object key)
        {
            return map.ContainsKey(tablename) && map[tablename].ContainsKey(key);
        }
        public static object Get(string tablename, object key)
        {
            if (map.ContainsKey(tablename) && HasKey(tablename, key))
                return map[tablename][key];
            return null;
        }
        public static T Get<T>(string tablename, object key)
        {
            return (T)Get(tablename, key);
        }
        public static void FlushAll()
        {
            map.Clear();
        }

        public static void Flush(string tablename)
        {
            if (map.ContainsKey(tablename))
                map[tablename].Clear();
        }

        public static void Dump(string path)
        {
            JSONHelper.ToFile(path, map);
        }
        public static void Load()
        {
            if(File.Exists(path))
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        map = (Map<string, Map<object, object>>)formatter.Deserialize(fs);
                    }
                }
                catch(Exception ex)
                {
                    Redox.Logger.LogError("[LocalStorage] Failed to load a storage because of error: " + ex.Message);
                }
            }
        }
        public static void Save()
        {
            try
            {
                using(FileStream fs = new FileStream(path, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, map);
                }
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError("[LocalStorage] Failed to save storage because of error: " + ex.Message);
            }
        }
    }
}
