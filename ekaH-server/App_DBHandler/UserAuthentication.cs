using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using ekaH_server.Models;
using ekaH_server.Models.UserAuth;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ekaH_server.App_DBHandler
{
    public class UserAuthentication
    {
        // Private variable for database connection
        
        public UserAuthentication()
        {
            
        }

        // This will return what the user type is. if it is student/faculty.
        public static bool getUserType(DBConnection db, string emailID)
        {
            MySqlDataReader reader = null;
            bool isStudent = true;

            string reqQuery = "select type from member_type where email='" + emailID + "';";

            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isStudent = reader.GetBoolean(0);
                }
                else
                {
                    reader.Dispose();
                    throw new Exception(Error.getInstance().getStringError(ErrorList.LOGIN_NO_USER));
                }
            }
            catch(MySqlException)
            {
                throw new Exception(Error.getInstance().getStringError(ErrorList.DATABASE_EXCEPTION));
            }

            reader.Dispose();

            return isStudent;


        }

        public static ErrorList verifyUserExists(DBConnection db, LogInInfo logInDetail)
        {
            int tempMemType = logInDetail.IsStudent ? 1 : 0;

            MySqlDataReader dataReader = null;

            string reqQuery = "select * from authentication where email='" + logInDetail.UserEmail + "';";

            //string response;
            ErrorList result;
            //MessageBox.Show(reqQuery);
            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
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
            catch (MySqlException ex)
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
            reqQuery += "insert into member_type values('" + registerDetail.userEmail + "', " + tempMemType + ");";

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