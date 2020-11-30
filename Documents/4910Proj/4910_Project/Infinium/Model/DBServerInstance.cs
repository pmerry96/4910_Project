using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Infinium.Model
{
    public class DBServerInstance {
        public DBServerInstance()
        {
        }
        private string databaseName = "InfDB2";
        //private string databaseName = "infTest_fel7";
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection getConnection(){
            return Connection;
        }
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBServerInstance _instance = null;
        public static DBServerInstance Instance()
        {
            if (_instance == null)
                _instance = new DBServerInstance();
            return _instance;
        }

        /**
        * This one is a bit confusing to understand, so here is my comment on usage 
         Parms:
            query : string - This is a string that is the skeleton of the SQL query to execute
            Targets : List<string> - This is a list of strings that outline the name of the targets parameters will be put into. The order should be the same as that listed within the query itself.
                Length = N
                If no parms, pass null
            Targets : List<string> - This is a list of strings to be inserted in place of the targets. The order should be the same as listed within the query itself
                Length = N
                If no parms, pass null
            Selectquery : bool - If this is a select query, then set to true. Otherwise, false. 
       */
        public MySqlDataReader ExecuteParameterizedQuery(string query, List<string> targets, List<string> parms, bool selectQuery)
        {
            if (parms != null && targets != null)
            {
                foreach (string s in parms)
                {
                    if (isInjection(s))
                    {
                        MessageBox.Show("You know hacking is not cool right? Keep trying, but I can do this all day.");
                        return null;
                    }
                }
            }
            LogQuery(query);
            int count = 0;
            MySqlCommand mySqlCommand = new MySqlCommand(query, Connection);
            if (targets != null && parms != null)
            {
                foreach (string i in targets)
                {
                    mySqlCommand.Parameters.AddWithValue(i, parms[count]);
                    count++;
                }
            }
            if(selectQuery)
            {
                return mySqlCommand.ExecuteReader();
            }
            else
            {
                mySqlCommand.ExecuteNonQuery();
            }
            return null;
        }

        private bool isInjection(string parms)
        {
            if (parms.Contains(";"))
                return true;
            return false;
        }

        public MySqlDataReader ExecuteQuery(string query, bool selectQuery)
        {
            LogQuery(query);

            if (selectQuery)
            {
                return SelectQuery(query);
            } else
            {
                UpdateQuery(query);
            }

            return null;
        }

        private void LogQuery(string query)
        {
            if (IsConnect())
            {
                string logQuery = "INSERT INTO Database_Log (Query) VALUES (@Query)";
                MySqlCommand cmd = new MySqlCommand(logQuery, Connection);
                cmd.Parameters.AddWithValue("@Query", query);
                cmd.ExecuteNonQuery();
            }
        }

        private MySqlDataReader SelectQuery(string query)
        {
            if (IsConnect())
            {
                MySqlCommand cmd = new MySqlCommand(query, Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                return rdr;
            }

            return null;
        }

        private void UpdateQuery(string query)
        {
            if (IsConnect())
            {
                var cmd = new MySqlCommand(query, Connection);
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsConnect()
        {
            if (Connection == null || Connection.State != System.Data.ConnectionState.Open)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;

                string connstring = string.Format("Server=inf2.cpxv7zhjkdkh.us-east-1.rds.amazonaws.com; database={0}; UID=phil; password=Ryamnaje", databaseName);
                //string connstring = string.Format("Server=mysql1.cs.clemson.edu; database={0}; UID=infTest_045z; password=B00zer01", databaseName);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }

            return true;
        }

        public void Close()
        {
            if (connection == null)
                return;

            connection.Close();
            connection = null;
        }
    }
}
