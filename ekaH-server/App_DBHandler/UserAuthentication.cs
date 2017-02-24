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
        
        public UserAuthentication()
        {
            
        }

        public string verifyUserExists(DBConnection db, LogInInfo logInDetail)
        {
            int tempMemType = logInDetail.isStudent ? 1 : 0;

            MySql.Data.MySqlClient.MySqlDataReader dataReader = null;

            string reqQuery = "select * from authentication where email='" + logInDetail.userEmail + "';";

            string response;
            //MessageBox.Show(reqQuery);
            try
            {
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(reqQuery, db.getConnection());
                dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    bool dbData = dataReader.GetValue(1).ToString() == "1" ? true : false;
                    if (logInDetail.pswd == dataReader.GetValue(2).ToString() && logInDetail.isStudent == dbData)
                    {
                        response = "success";
                    }
                    else
                    {
                        response = Error.getInstance().getStringError(ErrorList.LOGIN_WRONG_PASSWORD);
                    }
                }
                else
                {
                    response= Error.getInstance().getStringError(ErrorList.LOGIN_NO_USER);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                response= Error.getInstance().getStringError(ErrorList.DATABASE_EXCEPTION);
            }

            dataReader.Dispose();
            return response;
            
        }
       
    }
}