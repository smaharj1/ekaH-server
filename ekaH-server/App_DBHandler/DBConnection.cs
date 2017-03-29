using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace ekaH_server.App_DBHandler
{
    public class DBConnection
    {
        private static DBConnection database;

        private string connectionDetail = "server=localhost;port=5000;uid=root;pwd=nepal;database=ekah";

        private MySql.Data.MySqlClient.MySqlConnection connection;


        private DBConnection()
        {
            connectToDatabase();
        }

        public static DBConnection getInstance()
        {
            
            if (database != null)
            {
                return database;
            }
            
            database = new DBConnection();
            return database;
        }

        private void connectToDatabase()
        {
            try
            {
                connection = new MySql.Data.MySqlClient.MySqlConnection();
                connection.ConnectionString = connectionDetail;
                connection.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                // Handles the MySQL connection exception here.
                //MessageBox.Show("DB Connection failed");
            }
        }

        public MySql.Data.MySqlClient.MySqlConnection getConnection()
        {
            return connection;
        }
    }
}