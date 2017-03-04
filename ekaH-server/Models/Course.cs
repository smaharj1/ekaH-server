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
        public string CourseID { get; set; }
        public int Year { get; set; }
        public string Semester { get; set; }
        public string ProfessorID { get; set; }
        public string Days { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        

        public static ArrayList normalizeCourses(MySqlDataReader reader)
        {
            ArrayList courses = new ArrayList();
            while (reader.Read())
            {
                Course tempCourse = new Course();
                tempCourse.CourseID = FacultyWorker.getStringSafe(reader, 0);
                tempCourse.Year = (int)reader.GetValue(1);
                tempCourse.Semester = reader.GetString(2);
                tempCourse.ProfessorID = reader.GetString(3);
                tempCourse.Days = reader.GetString(4);
                tempCourse.StartTime = reader.GetTimeSpan(5);
                tempCourse.EndTime = reader.GetTimeSpan(6);
                tempCourse.CourseName = FacultyWorker.getStringSafe(reader, 7);
                tempCourse.CourseDescription = FacultyWorker.getStringSafe(reader, 8);

                courses.Add(tempCourse);
            }

            // Returns all of the courses arranged in Course object.
            reader.Dispose();
            return courses;
            
        }
    }
}