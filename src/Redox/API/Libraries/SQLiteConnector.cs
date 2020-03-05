using System;
using System.IO;
using System.Data.SQLite;

namespace Redox.API.Libraries
{
    public class SQLiteConnector
    {
        private static SQLiteConnector _instance;
        private readonly string _path = Path.Combine(Redox.DefaultPath, "RedoxSQL.sqlite");
        private  SQLiteConnection _connection;
     
        public static SQLiteConnector GetInstance()
        {
            if (_instance == null)
                _instance = new SQLiteConnector();
            return _instance;
        }

        public SQLiteConnector()
        {
            if (!File.Exists(_path))
                SQLiteConnection.CreateFile(_path);
        }

        public SQLiteConnection Connect(string arguments = ";Version3;")
        {
            _connection = new SQLiteConnection(string.Format("Data Source={0}{1}", _path, arguments));
            _connection.Open();
            return _connection;
        }

        public SQLiteCommand CreateCommand(string commandString)
        {
            return new SQLiteCommand(commandString, _connection);
        }
    }
}
