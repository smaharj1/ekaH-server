using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using ekaH_server.Models;
using System.Windows.Forms;

namespace ekaH_server.App_DBHandler
{
    public class UserAuthentication
    {
        // Private variable for database connection
        private MySql.Data.MySqlClient.MySqlConnection connection;
        
        public UserAuthentication()
        {
            
        }

        public static Boolean verifyUser(DBConnection db, LogInInfo logInDetail)
        {
            int tempMemType = logInDetail.isStudent ? 1 : 0;

            MySql.Data.MySqlClient.MySqlDataReader dataReader = null;

            string reqQuery = "select * from authentication where email='" + logInDetail.userEmail + "' and " +
                "member_type=" + tempMemType + " and pswd='" + logInDetail.pswd + "';";

            //MessageBox.Show(reqQuery);

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(reqQuery, db.getConnection());
            dataReader = cmd.ExecuteReader();

            if (dataReader.Read())
            {


                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}