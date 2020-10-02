using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MyAccountPage
{
    class DatabaseConnect
    {
        private DatabaseConnect()
        {
        }

        public string DatabaseName { get; set; } = "infTest_fel7";

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DatabaseConnect _instance = null;
        public static DatabaseConnect Instance()
        {
            if (_instance == null)
                _instance = new DatabaseConnect();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null || Connection.State != System.Data.ConnectionState.Open)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;

                string connstring = string.Format("Server=mysql1.cs.clemson.edu; database={0}; UID=infTest_045z; password=B00zer01", DatabaseName);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }

            return true;
        }

        public void Close()
        {
            connection.Close();
            connection = null;
        }
    }
}
