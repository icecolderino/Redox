using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Redox.API.Libraries.SQLite
{
    public sealed class SQLite
    {
        internal readonly SQLiteConnection connection;
        private readonly string _filepath;

        public bool IsOpen
        {
            get
            {
                return connection?.State == ConnectionState.Open;
            }
        }
        public SQLite(string name, string password = "")
        {       
            _filepath = Path.Combine(Redox.DataPath, name);
            if(!File.Exists(_filepath))
                SQLiteConnection.CreateFile(_filepath);
            connection = new SQLiteConnection($"Data Source={_filepath}; Version=3; Password={password};");
            
        }


        public bool Open()
        {
            if(!IsOpen)
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SQLiteException ex)
                {
                    Redox.Logger.LogError($"[SQLite] Failed to open connection to {Path.GetFileName(_filepath)}, error: " + ex.Message);
                    return false;
                }
            }
            return true;
        }     
        public void Close()
        {
            if(IsOpen)
                connection.Close();
        }

        public void CreateTable(string name, string[] colums)
        {
            if(IsOpen)
            {
                string query = string.Format("create table if not exists {0} ({1})", name, string.Join(",", colums));
                ExecuteNonQueryCommand(query);
            }
        }
        public void Insert(string name, Dictionary<string, object> records)
        {
            if(IsOpen)
            {
                string query = string.Format("INSERT INTO {0} ({1}) VALUES({2})", name, string.Join(",", records.Keys.ToArray()), string.Join(",", ParseValues(records.Values.ToArray())));
                ExecuteNonQueryCommand(query);
            }
        }
        private object[] ParseValues(object[] values)
        {
            var list = new List<object>();
            foreach(object value in values)
            {
                if(value is string)
                {
                    list.Add($"'{value.ToString()}'");
                    continue;
                }
                list.Add(value);
            }
            return list.ToArray();
        }    
        private void ExecuteNonQueryCommand(string query)
        {
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch(SQLiteException ex)
            {
                var reader = GetReader("x");
                Redox.Logger.LogError("[SQLite] Failed to execute non query command, error: " + ex.Message);
            }           
        }    
        
        private SQLiteDataReader GetReader(string query)
        {
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                return command.ExecuteReader();
            }
        }
    }
}
