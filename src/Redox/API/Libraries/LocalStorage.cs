using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using Redox.API.Collections;

using UnityEngine;
using System.Threading.Tasks;

namespace Redox.API.Libraries
{
    /// <summary>
    /// Represents a localstore data file
    /// </summary>
    public class LocalStorage 
    {
        private readonly Hashtable table = new Hashtable();
        private readonly string path = Path.Combine(Redox.DefaultPath, "Redox.storage");

        private static LocalStorage instance;

        public static LocalStorage GetStorage()
        {
            
            if (instance == null)
                instance = new LocalStorage();
            return instance;
        }

        public LocalStorage()
        {
            Redox.Logger.LogInfo("[LocalStorage] Loading data..");
            if(File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    table = formatter.Deserialize(fs) as Hashtable;
                }
            }
        }


        public void Append(string tablename, object key, object value)
        {
            if(key != null)
            {
                Stringify(ref value);


                if (!(table[tablename] is Hashtable hash))
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
                if (table[tablename] is Hashtable hash)
                {
                    hash.Remove(key);
                }
            }
        }

        public void SetValue(string tablename, object key, object value)
        {
            if((key != null) && (value != null))
            {
                Stringify(ref value);

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
                if (table[tablename] is Hashtable hash)
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
            if (table[tablename] is Hashtable hash)
            {
                return hash.ContainsKey(key);
            }
            return false;
        }

        public object[] GetKeys (string tablename)
        {
            if (table[tablename] is Hashtable hash)
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

        public HashMap<object, object> ToMap(string tablename)
        {
            if (table[tablename] is Hashtable hash)
            {
                var dict = new HashMap<object, object>();

                foreach (KeyValuePair<object, object> pair in hash)
                {
                    dict.Add(pair.Key, pair.Value);
                }
                return dict;
            }
            return new HashMap<object, object>();
        }

        public HashMap<object, T> FilterType<T>(string tablename)
        {
            if (table[tablename] is Hashtable hash)
            {
                var dict = new HashMap<object, T>();

                foreach (KeyValuePair<object, object> pair in hash)
                {
                    if (pair.Value is T)
                        dict.Add(pair.Key, (T)pair.Value);

                }
                return dict;
            }
            return new HashMap<object, T>();
        }

        public void ClearTable(string tablename)
        {
            if (table[tablename] is Hashtable hash)
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

        private void Stringify(ref object value)
        {
            if((value is Vector2) || (value is Vector3) || (value is Vector4) || (value is Quaternion))
                value = value.ToString();
        }
    }
}
