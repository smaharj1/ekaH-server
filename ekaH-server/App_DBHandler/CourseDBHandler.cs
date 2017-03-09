using ekaH_server.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.App_DBHandler
{
    public class CourseDBHandler
    {



        public static bool courseExists(Course course)
        {

            return courseExists(course.CourseID);
            
        }

        public static bool courseExists(string courseID)
        {
            DBConnection db = DBConnection.getInstance();

            string reqQuery = "select * from courses where courseID='" + courseID + "';";

            MySqlDataReader reader = null;

            bool exists = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    exists = true;
                }

            }
            catch (MySqlException)
            {
                throw new Exception(Error.getInstance().getStringError(ErrorList.DATABASE_EXCEPTION));
            }

            reader.Dispose();
            return exists;
        }

        // Returns all the courses taught by the faculty member.
        public static MySqlDataReader readCoursesByFaculty(string emailID)
        {
            DBConnection db = DBConnection.getInstance();

            MySqlDataReader reader = null;

            string reqQuery = "select * from courses where professorID='" + emailID + "';";

            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                reader = cmd.ExecuteReader();

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message); 
                return null;
            }

            return reader;

        }

        // This posts a course in to the database.
        public static bool executePostCourse(Course course)
        {
            DBConnection db = DBConnection.getInstance();

            string reqQuery = "insert ignore into courses(courseID, year, semester, professorID, days, startTime, endTime, courseName, courseDescription) " +
                "values('" + course.CourseID + "', " + course.Year + ", '" + course.Semester + "', '" + course.ProfessorID + "', '" +
                course.Days + "', '" + course.StartTime + "', '" + course.EndTime + "', '" + course.CourseName + "', '" + course.CourseDescription + "');";

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

        public static bool executePutCourse(Course course, string prevCourseID)
        {
            DBConnection db = DBConnection.getInstance();

            string reqQuery = "update courses set courseID='" + course.CourseID+"', year = " + course.Year + ", semester = '" + course.Semester + "', days = '" + 
                course.Days + "', startTime = '" + course.StartTime.ToString() + "', endTime = '" + course.EndTime.ToString() +
                "', courseName = '" + course.CourseName + "', courseDescription = '" + course.CourseDescription + "' where courseID='"+ prevCourseID +"';";

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

        public static bool executeDeleteCourse(string courseID)
        {
            DBConnection db = DBConnection.getInstance();

            string reqQuery = "delete from courses where courseID='" + courseID + "';";

            // First, check if the professor teaches the course s/he wants to delete.
            string checkQuery = "select * from courses where courseID='" + courseID + "';";

            MySqlDataReader reader = null;

            try
            {
                
                MySqlCommand cmd = new MySqlCommand(checkQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    // In this case the course is not taught by the professor.
                    throw new Exception("The access to this course is forbidden for the user.");
                }

            }
            catch (MySqlException)
            {
                return false;
            }

            reader.Dispose();
            

            // Then, execute the delete.
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