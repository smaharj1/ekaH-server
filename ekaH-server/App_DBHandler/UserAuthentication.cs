using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using ekaH_server.Models;
using ekaH_server.Models.UserAuth;
using System.Windows.Forms;

namespace ekaH_server.App_DBHandler
{
    public class UserAuthentication
    {
        // Private variable for database connection
        
        public UserAuthentication()
        {
            
        }

        public static ErrorList verifyUserExists(DBConnection db, LogInInfo logInDetail)
        {
            int tempMemType = logInDetail.IsStudent ? 1 : 0;

            MySql.Data.MySqlClient.MySqlDataReader dataReader = null;

            string reqQuery = "select * from authentication where email='" + logInDetail.UserEmail + "';";

            //string response;
            ErrorList result;
            //MessageBox.Show(reqQuery);
            try
            {
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(reqQuery, db.getConnection());
                dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    bool dbData = dataReader.GetValue(1).ToString() == "1" ? true : false;
                    if (logInDetail.Pswd == dataReader.GetValue(2).ToString())
                    {
                        if (logInDetail.IsStudent == dbData)
                        {
                            result = ErrorList.SUCCESS;

                        }
                        else
                        {
                            result = ErrorList.USER_TYPE_ERROR;
                        }
                    }
                    else
                    {
                        result = ErrorList.LOGIN_WRONG_PASSWORD;
                    }
                }
                else
                {
                    result = ErrorList.LOGIN_NO_USER;
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                result= ErrorList.DATABASE_EXCEPTION;
            }

            dataReader.Dispose();

            return result;
            
        }


        public static ErrorList registerUser(DBConnection database, RegisterInfo registerDetail)
        {
            int tempMemType = registerDetail.isStudent ? 1 : 0;
            string reqQuery;
            ErrorList result;

            reqQuery = "insert into authentication values('" + registerDetail.userEmail + "', " + tempMemType + ", '" + registerDetail.pswd + "');";

            if (registerDetail.isStudent)
            {
                reqQuery += "insert into student_info(firstName, lastName, email, graduationYear) values('" +
                    registerDetail.firstName + "', '" + registerDetail.lastName + "', '" + registerDetail.userEmail + "', " + registerDetail.extraInfo + ");";

            }
            else
            {
                reqQuery += "insert into professor_info(firstName, lastName, email, Department) values('" +
                    registerDetail.firstName + "', '" + registerDetail.lastName + "', '" + registerDetail.userEmail + "', '" + registerDetail.extraInfo + "');";
            }

            try
            {
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(reqQuery, database.getConnection());
                cmd.ExecuteNonQuery();

                result = ErrorList.SUCCESS;
            }
            catch(MySql.Data.MySqlClient.MySqlException)
            {
                result = ErrorList.DATABASE_EXCEPTION;
            }
            

            return result;
        }
       
    }
}