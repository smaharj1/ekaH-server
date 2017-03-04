using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using ekaH_server.Models.UserModels;
using System.Windows.Forms;

namespace ekaH_server.App_DBHandler
{
    public class FacultyDBHandler
    {
        private FacultyDBHandler()
        {

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

        public static MySqlDataReader executeFacultyInfoQuery(string emailID)
        {
            DBConnection db = DBConnection.getInstance();

            MySqlDataReader dataReader = null;

            string reqQuery = "select * from professor_info where email='" + emailID + "';";

            //string response;
            //ErrorList result;

            // MessageBox.Show(reqQuery);
            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                dataReader = cmd.ExecuteReader();

                
            }
            catch (MySqlException)
            {
                //dataReader.Dispose();
                return null;
            }

            return dataReader;
        }

        // This posts a course in to the database.
        public static bool executePostCourse(Course course)
        {
            DBConnection db = DBConnection.getInstance();

            string reqQuery = "insert into courses(courseID, year, semester, professorID, days, startTime, endTime, courseName, courseDescription) " +
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

        // This method changes the faculty info
        public static bool executePutFacultyInfo(FacultyInfo facultyMem)
        {

            DBConnection db = DBConnection.getInstance();

            string reqQuery = "update professor_info set firstName = '" + facultyMem.FirstName + "', lastName = '" + facultyMem.LastName + "', department = '" + 
                facultyMem.Department + "', education = '" + facultyMem.Education +
                "', university = '" + facultyMem.University + "', concentration = '" + facultyMem.Concentration + "', streetAdd1 = '" + facultyMem.StreetAdd1 + 
                "', streetAdd2 = '" + facultyMem.StreetAdd2 + "', state = '" + facultyMem.State + "', zip = '" + facultyMem.Zip + "' where email = '" + facultyMem.Email + "';";

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


    }
}