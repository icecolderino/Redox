using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Redox;
using MySql.Data.MySqlClient;

namespace Redox.API.Libraries
{
    public class MySQL
    {
        private MySqlConnection connection;
        private static MySQL _instance;

        public static MySQL GetInstance()
        {
            if(_instance == null)
                _instance = new MySQL();
            return _instance;
        }

        //Gets connection info from Redox.json file
        public void SetupNewConnection()
        {
            connection = new MySqlConnection("SERVER=" + (string)Redox.config.Mysql["ServerIP"] + ";" + "DATABASE=" +
            (string)Redox.config.Mysql["Database"] + ";" + "UID=" + (string)Redox.config.Mysql["Username"] + ";" + "PASSWORD=" + (string)Redox.config.Mysql["Password"] + ";");
        }

        //Plugins can change the connection if needed
        public void SetupNewConnection(string server, string database, string uid, string password)
        {
            connection = new MySqlConnection("SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Redox.Logger.LogError("MySQL: Cannot connect to server.");
                        break;

                    case 1045:
                        Redox.Logger.LogError("MySQL: Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Redox.Logger.LogError("MySQL: Failed to close connection.");
                Redox.Logger.LogError("Reason: " + ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(string query)
        {
            //example query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')"

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update(string query)
        {
            //example query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'"

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete(string query)
        {
            //example query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public List<string>[] Select(string query)
        {
            //example query = "SELECT * FROM tableinfo";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["ID"] + "");
                    list[1].Add(dataReader["Key"] + "");
                    list[2].Add(dataReader["Value"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count(string query)
        {
            //example query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
    }
}
