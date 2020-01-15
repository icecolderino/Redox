using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using System.Threading.Tasks;

namespace Redox.API.Libraries
{
    public class DataStore 
    {
        private readonly Hashtable table = new Hashtable();
        private readonly string path = Path.Combine(Redox.DefaultPath, "Datastore.ds");

        private static DataStore instance;

        public static DataStore GetInstance()
        {
            if (instance == null)
                instance = new DataStore();
            return instance;
        }

        public DataStore()
        {
            Redox.Logger.LogInfo("[DataStore] Loading data..");
            if(File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    table = formatter.Deserialize(fs) as Hashtable;
                    fs.Dispose();
                }
            }
        }


        public void Append(string tablename, object key, object value)
        {
            if(key != null)
            {
                stringify(ref value);

                Hashtable hash = table[tablename] as Hashtable;

                if(hash == null)
                {
                    hash = new Hashtable();
                    table.Add(tablename, hash);
                }
                hash[key] = value;
            }
        }
     
        public void Remove(string tablename, object key)
        {
            if(key != null)
            {
                Hashtable hash = table[tablename] as Hashtable;

                if(hash != null)
                {
                    hash.Remove(key);
                }
            }
        }

        public void SetValue(string tablename, object key, object value)
        {
            if((key != null) && (value != null))
            {
                stringify(ref value);

                Hashtable hash = table[tablename] as Hashtable;

                if(hash != null)
                {
                    hash[key] = value;
                    return;
                }
                hash.Add(key, value);
            }
        }


        public object GetValue(string tablename, object key)
        {
            if(key != null)
            {
                Hashtable hash = table[tablename] as Hashtable;

                if(hash != null)
                {
                    return hash[key];
                }
                return null;
            }
            return null;
        }

        public bool TryGetValue(string tablename, object key, out object value)
        {
            object result = GetValue(tablename, key);

            if(result != null)
            {
                value = result;
                return true;
            }
            value = null;
            return false;
        }

        public bool ContainsKey(string tablename, object key)
        {
            Hashtable hash = table[tablename] as Hashtable;
            if (hash != null)
            {
                return hash.ContainsKey(key);
            }
            return false;
        }

        public object[] GetKeys (string tablename)
        {

            Hashtable hash = table[tablename] as Hashtable;
            if(hash != null)
            {
                List<object> keys = new List<object>();

                foreach (object key in hash.Keys)
                {
                    keys.Add(key);
                }
                return keys.ToArray();
            }
            return new object[0];
           
        }

        public Dictionary<object, object> ToDictionary(string tablename)
        {
            Hashtable hash = table[tablename] as Hashtable;
            if (hash != null)
            {
                var dict = new Dictionary<object, object>();

                foreach(KeyValuePair<object, object> pair in hash)
                {
                    dict.Add(pair.Key, pair.Value);
                }
                return dict;
            }
            return new Dictionary<object, object>();
        }

        public Dictionary<object, object> FilterType<T>(string tablename)
        {
            Hashtable hash = table[tablename] as Hashtable;
            if (hash != null)
            {
                var dict = new Dictionary<object, object>();

                foreach(KeyValuePair<object, object> pair in hash)
                {
                    if (pair.Value is T)
                        dict.Add(pair.Key, pair.Value);

                }
                return dict;
            }
            return new Dictionary<object, object>();
        }

        public void ClearTable(string tablename)
        {
            Hashtable hash = table[tablename] as Hashtable;
            if(hash != null)
            {
                hash.Clear();
            }
        }

        public async Task Save()
        {
            await Task.Run(() =>
            {
                File.Delete(path);

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, table);
                    fs.Dispose();
                }
            });
           
        }

        private void stringify(ref object value)
        {
            if((value is Vector2) || (value is Vector3) || (value is Vector4) || (value is Quaternion))
                value = value.ToString();
        }
    }
}
