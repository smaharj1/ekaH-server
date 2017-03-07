using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ekaH_server.App_DBHandler
{
    public class CourseStudentDBHandler
    {
        public static List<string> getAllStudentsFromDB(string cid)
        {
            DBConnection db = DBConnection.getInstance();

            string reqQuery = "select * from studentcourse where courseID='" + cid + "';";

            MySqlDataReader reader = null;
            List<string> result = new List<string>();

            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                reader = cmd.ExecuteReader();
            }
            catch (MySqlException)
            {
                throw new Exception();
            }

            while(reader.Read())
            {
                result.Add(reader.GetString(1));
            }

            reader.Dispose();
            return result;
        }

        public static bool deleteStudentFromDB(string courseID, string studentID)
        {
            DBConnection db = DBConnection.getInstance();
            string reqQuery = "delete from studentcourse where courseID='" + courseID + "' and studentID='" + studentID + "';";

            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                return false;
            }

            return true;
        }

        public static bool addStudentsToDB(string courseID, List<string> studentEmails)
        {
            DBConnection db = DBConnection.getInstance();
            string reqQuery = "";
            foreach (string email in studentEmails)
            {
                reqQuery += "insert into studentcourse(courseID, studentID) values('" + courseID + "', '" + email + "');";
            }

            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                cmd.ExecuteNonQuery();
            }
            catch(MySqlException)
            {
                return false;
            }
            return true;

        }
    }
}