using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Redox.API.Libraries
{
    public sealed class SQLite
    {
        public readonly SQLiteConnection Connection;

        public FileInfo FilePath { get; }

        public bool IsOpen
        {
            get
            {
                return Connection?.State == ConnectionState.Open;
            }
        }
        public string Name
        {
            get
            {
                return FilePath?.Name;
            }
        }

        public SQLite(string name, string password = "")
        {
            string path = Path.Combine(Redox.DataPath, name);
            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);
            FilePath = new FileInfo(path);

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = path;
            builder.Version = 3;
            builder.Password = password;

            Connection = new SQLiteConnection(builder.ConnectionString);
        }

        public bool Open()
        { 
            try
            {
                if(!IsOpen)
                {
                    Connection.Open();
                    return true;
                }
                return true;
            }
            catch(SQLiteException ex)
            {
                Redox.Logger.LogError($"[SQLite] Failed to open connection to {Name} because of error: {ex.Message}");
                return false;
            }
        }

     
        public async Task<bool> OpenAsync()
        {
            return await Task.Run(() => Open());
        }

        public void Close()
        {
            try
            {
                if(IsOpen)
                {
                    Connection.Close();
                }
            }
            catch(SQLiteException ex)
            {
                Redox.Logger.LogError($"[SQLite] Failed to close _connection of {Name} because of error: {ex.Message}");
            }
        }

        public async Task CloseAsync()
        {
            await Task.Run(() => Close());
        }

        public SQLiteCommand CreateCommand()
        {
            return new SQLiteCommand(Connection);
        }
    }
}
