using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Redox.API.Libraries
{

    /// <summary>
    /// LocalStorage for redox
    /// </summary>
    public static class LocalStorage
    {
        private static readonly string path = Path.Combine(Bootstrap.RedoxMod.RootPath, "Storage.data");
        private static Hashtable Table = new Hashtable();


        public static void Add(string tablename, object key, object value)
        {
            if (string.IsNullOrEmpty(tablename) || (key == null))
                return;
            Hashtable table = Table[tablename] as Hashtable;
            if (table == null)
                Table.Add(tablename, new Hashtable());
            table[key] = value;
        }

        public static void Remove(string tablename, object key)
        {
            if (string.IsNullOrEmpty(tablename) || (key == null))
                return;

            Hashtable table = Table[tablename] as Hashtable;
            if (table != null)
                table.Remove(key);
        }
        public static bool HasKey(string tablename, object key)
        {
            if (string.IsNullOrEmpty(tablename) || (key == null))
                return false;
            Hashtable table = Table[tablename] as Hashtable;

            return table[key] != null;
        }
        public static object Get(string tablename, object key)
        {
            if (string.IsNullOrEmpty(tablename) || (key == null))
                return null;
            Hashtable table = Table[tablename] as Hashtable;
            if (table != null)
                return table[key];
            return null;
        }
        public static T Get<T>(string tablename, object key)
        {
            return (T)Get(tablename, key);
        }
        public static void FlushAll()
        {
            Table.Clear();
            Bootstrap.RedoxMod.Logger.LogInfo("[LocalStorage] The database has been flushed.");
        }

        public static void Flush(string tablename)
        {
            if (string.IsNullOrEmpty(tablename))
                return;
            Hashtable table = Table[tablename] as Hashtable;
            if (table != null)
                Table.Remove(tablename);
        }
        internal static void Load()
        {
            if(File.Exists(path))
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        Table = (Hashtable)formatter.Deserialize(fs);
                    }
                }
                catch(Exception ex)
                {
                    Bootstrap.RedoxMod.Logger.LogError("[LocalStorage] Failed to load a storage because of error: " + ex.Message);
                    Bootstrap.RedoxMod.Logger.LogWarning("[LocalStorage] Please remove the Storage data file in the redox directory.");
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
                    formatter.Serialize(fs, Table);
                }
            }
            catch(Exception ex)
            {
                Bootstrap.RedoxMod.Logger.LogError("[LocalStorage] Failed to save storage because of error: " + ex.Message);
            }
        }
    }
}
