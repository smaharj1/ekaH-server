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
        public static bool getUserType(string emailID)
        {
            DBConnection db = DBConnection.getInstance();
            MySqlDataReader reader = null;
            bool isStudent = true;

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = db.getConnection();
                cmd.CommandText = "select type from member_type where email=@emailID;";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("emailID", emailID);

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
            catch(MySqlException ex)
            {
                string mes = ex.Message;
                throw new Exception(Error.getInstance().getStringError(ErrorList.DATABASE_EXCEPTION));
            }

            reader.Dispose();

            return isStudent;


        }

        public static ErrorList verifyUserExists(DBConnection db, LogInInfo logInDetail)
        {
            int tempMemType = logInDetail.IsStudent ? 1 : 0;

            MySqlDataReader dataReader = null;

            ErrorList result;

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = db.getConnection();
                cmd.CommandText = "select * from authentication where email = @emailID;";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("emailID", logInDetail.UserEmail);

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
            ErrorList result;

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = database.getConnection();

                cmd.CommandText = "insert into authentication values(@emailID, @memType, @password);"
                    + "insert into member_type values(@emailID, @memType);";

                if (registerDetail.isStudent)
                {
                    cmd.CommandText += "insert into student_info(firstName, lastName, email, graduationYear) values(@firstName, @lastName," +
                        "@emailID, @extraInfo);";
                }
                else
                {
                    cmd.CommandText += "insert into professor_info(firstName, lastName, email, Department) values(@firstName, @lastName," +
                        "@emailID, @extraInfo);";
                }
                cmd.Prepare();

                cmd.Parameters.AddWithValue("emailID", registerDetail.userEmail);
                cmd.Parameters.AddWithValue("memType", tempMemType);
                cmd.Parameters.AddWithValue("password", registerDetail.pswd);
                cmd.Parameters.AddWithValue("firstName", registerDetail.firstName);
                cmd.Parameters.AddWithValue("lastName", registerDetail.lastName);
                cmd.Parameters.AddWithValue("extraInfo", registerDetail.extraInfo);

                cmd.ExecuteNonQuery();

                result = ErrorList.SUCCESS;
            }
            catch(MySqlException)
            {
                result = ErrorList.DATABASE_EXCEPTION;
            }
            

            return result;
        }
       
    }
}