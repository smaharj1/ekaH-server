using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;

namespace ekaH_server.App_DBHandler
{
    public class UserAuthentication
    {
        // Private variable for database connection
        private MySql.Data.MySqlClient.MySqlConnection connection;
        private string connectionDetail = "server=127.0.0.1;uid=root;pswd=nepal;database=ekah";

        public UserAuthentication()
        {
            try
            {
                connection = new MySql.Data.MySqlClient.MySqlConnection();
                connection.ConnectionString = connectionDetail;
                connection.Open();

                Console.WriteLine("DB Connection req");
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                // Handles the MySQL connection exception here.
                Console.WriteLine("DB Connection failed");
            }
        }
       
    }
}