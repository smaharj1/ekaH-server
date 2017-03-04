using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections;
using ekaH_server.Models.UserModels;

namespace ekaH_server.Models
{
    public class Course : Object
    {
        public Int64 CourseID { get; set; }
        public string CourseCode { get; set; }
        public int Year { get; set; }
        public string Semester { get; set; }
        public string ProfessorID { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        

        public static ArrayList normalizeCourses(MySqlDataReader reader)
        {
            ArrayList courses = new ArrayList();
            while (reader.Read())
            {
                Course tempCourse = new Course();
                tempCourse.CourseID = reader.GetInt64(0);
                tempCourse.CourseCode = reader.GetString(1);
                tempCourse.Year = (int)reader.GetValue(2);
                tempCourse.Semester = reader.GetString(3);
                tempCourse.ProfessorID = reader.GetString(4);
                tempCourse.CourseName = FacultyWorker.getStringSafe(reader, 5);
                tempCourse.CourseDescription = FacultyWorker.getStringSafe(reader, 6);

                courses.Add(tempCourse);
            }

            // Returns all of the courses arranged in Course object.
            reader.Dispose();
            return courses;
            
        }
    }
}