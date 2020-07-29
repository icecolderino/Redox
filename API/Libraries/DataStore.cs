using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Redox.Core.Data;

namespace Redox.API.Libraries
{

    /// <summary>
    /// DataStore for redox
    /// </summary>
    public class DataStore : IData
    {
        private readonly string _path = Path.Combine(Bootstrap.RedoxMod.RootPath, "Storage.data");
        private Hashtable _table = new Hashtable();
        
        public bool Exists => File.Exists(_path);
        
        /// <summary>
        /// Adds a new key & value to a table.
        /// </summary>
        /// <param name="tablename">The name of the table.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string tablename, object key, object value)
        {
            if (string.IsNullOrEmpty(tablename) || (key == null))
                return;
            Hashtable table = _table[tablename] as Hashtable;
            if (table == null)
                _table.Add(tablename, new Hashtable());
            table[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="key"></param>
        public void Remove(string tablename, object key)
        {
            if (string.IsNullOrEmpty(tablename) || (key == null))
                return;

            Hashtable table = _table[tablename] as Hashtable;
            if (table != null)
                table.Remove(key);
        }
        /// <summary>
        /// Checks if a table has contains the specified key.
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasKey(string tablename, object key)
        {
            if (string.IsNullOrEmpty(tablename) || (key == null))
                return false;
            Hashtable table = _table[tablename] as Hashtable;

            return table[key] != null;
        }
        /// <summary>
        /// Returns the value that is associated with the given key.
        /// </summary>
        /// <param name="tablename">The name of the table</param>
        /// <param name="key">The key of the value.</param>
        /// <returns></returns>
        public object Get(string tablename, object key)
        {
            if (string.IsNullOrEmpty(tablename) || (key == null))
                return null;
            Hashtable table = _table[tablename] as Hashtable;
            if (table != null)
                return table[key];
            return null;
        }
        /// <summary>
        /// Returns the value that is associated with the given key.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="tablename">The name of the table.</param>
        /// <param name="key">The key of the value.</param>
        /// <returns></returns>
        public T Get<T>(string tablename, object key)
        {
            return (T)Get(tablename, key);
        }
        public void FlushAll()
        {
            _table.Clear();
            Bootstrap.RedoxMod.Logger.LogInfo("[LocalStorage] The database has been flushed.");
        }

        /// <summary>
        /// Flushes a table.
        /// </summary>
        /// <param name="tablename">The table you want to flush.</param>
        public void Flush(string tablename)
        {
            if (string.IsNullOrEmpty(tablename))
                return;
            Hashtable table = _table[tablename] as Hashtable;
            if (table != null)
                _table.Remove(tablename);
        }
        public Task LoadAsync()
        {
            if(File.Exists(_path))
            {
                try
                {
                    using (FileStream fs = new FileStream(_path, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        _table = (Hashtable)formatter.Deserialize(fs);
                    }
                }
                catch(Exception ex)
                {
                    Bootstrap.RedoxMod.Logger.LogError("[LocalStorage] Failed to load a storage because of error: " + ex.Message);
                    Bootstrap.RedoxMod.Logger.LogWarning("[LocalStorage] Please remove the Storage data file in the redox directory.");
                }
            }
            return Task.CompletedTask;
        }

        

        public Task SaveAsync()
        {
            try
            {
                using(FileStream fs = new FileStream(_path, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, _table);
                }
            }
            catch(Exception ex)
            {
                Bootstrap.RedoxMod.Logger.LogError("[LocalStorage] Failed to save storage because of error: " + ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
