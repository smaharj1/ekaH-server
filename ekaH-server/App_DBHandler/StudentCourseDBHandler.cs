using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using ekaH_server.Models;
using System.Collections;

namespace ekaH_server.App_DBHandler
{
    public class StudentCourseDBHandler
    {
       

        public static ArrayList getAllCoursesByStudent(string studentID)
        {
            DBConnection db = DBConnection.getInstance();

            string reqQuery = "select * from courses where courseID in (select courseID from studentcourse where studentID = '" + studentID + "');";

            MySqlDataReader reader = null;

            ArrayList courses;
            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                courses = Course.normalizeCourses(reader);
            }
            catch(MySqlException)
            {
                throw new Exception();
            }

            return courses;
        }

        public static bool deleteStudentFromCourse(string courseID, string studentID)
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
                reqQuery += "insert ignore into studentcourse(courseID, studentID) values('" + courseID + "', '" + email + "');";
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

        public static bool addOneStudentToCourse(string courseID, string studentID)
        {
            DBConnection db = DBConnection.getInstance();
            string reqQuery = "insert ignore into studentcourse(courseID,studentID) values('" + courseID + "', '" + studentID + "');";

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

        public static bool studentAlreadyExists(string courseID, string studentID)
        {
            DBConnection db = DBConnection.getInstance();
            string reqQuery = "select * from studentcourse where courseID='" + courseID + "' and studentID='" + studentID + "';";

            MySqlDataReader reader = null;

            bool result = false;
            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    result = true;
                }
            }
            catch(MySqlException)
            {
                throw new Exception();
            }

            reader.Dispose();
            return result;
        }
    }
}