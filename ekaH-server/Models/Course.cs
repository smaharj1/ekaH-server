using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
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

        public static void fixCourseObject(ref Course course)
        {
            // Some of them would be strict. So, they don't need fixing like year.
            course.Semester = course.Semester.ToUpper();
            course.Days = course.Days.ToUpper();

            course.CourseName = course.CourseName == null ? "" : course.CourseName;
            course.CourseDescription = course.CourseDescription == null ? "" : course.CourseDescription;

            generateUniqueCourseID(ref course);
        }

        private static void generateUniqueCourseID(ref Course course)
        {
            // At this point we are assuming that the Course object provided will have all upper case.
            StringBuilder uniqueID = new StringBuilder("CRS-");

            uniqueID.Append(course.Semester);
            uniqueID.Append(course.Year);
            string[] username = course.ProfessorID.Split('@');
            uniqueID.Append(username[0]);

            uniqueID.Append(course.StartTime.Hours);
            uniqueID.Append(course.StartTime.Minutes);
            uniqueID.Append(course.Days);

            course.CourseID = uniqueID.ToString();

        }

        public bool validateFields()
        {
            // Email is already validated at this point.
            if (Year == null || Year.ToString().Length != 4) { return false; }

            Semester = Semester.ToUpper();

            if (Semester == null || (Semester != "F" && Semester != "S")) return false;

            if (Days == null || Days.Length > 7) return false;

            if (StartTime.Hours > 24 || StartTime.Minutes > 59) return false;
            if (EndTime.Hours > 24 || EndTime.Minutes > 59) return false;

            return true;
        }


    }
}