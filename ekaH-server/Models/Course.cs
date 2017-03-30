using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections;
using ekaH_server.Models.UserModels;
using ekaH_server.App_DBHandler;

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
        

        public static List<Course> normalizeCourses(MySqlDataReader reader)
        {
            List<Course> courses = new List<Course>();
            while (reader.Read())
            {
                Course tempCourse = new Course();
                tempCourse.CourseID = UserWorker.getStringSafe(reader, 0);
                tempCourse.Year = (int)reader.GetValue(1);
                tempCourse.Semester = reader.GetString(2);
                tempCourse.ProfessorID = reader.GetString(3);
                tempCourse.Days = reader.GetString(4);
                tempCourse.StartTime = reader.GetTimeSpan(5);
                tempCourse.EndTime = reader.GetTimeSpan(6);
                tempCourse.CourseName = UserWorker.getStringSafe(reader, 7);
                tempCourse.CourseDescription = UserWorker.getStringSafe(reader, 8);

                courses.Add(tempCourse);
            }

            // Returns all of the courses arranged in Course object.
            reader.Dispose();
            return courses;
            
        }

        public static void fixCourseObject(ref cours course)
        {
            // Some of them would be strict. So, they don't need fixing like year.
            course.semester = course.semester.ToUpper();
            course.days = course.days.ToUpper();

            course.courseName= course.courseName == null ? "" : course.courseName;
            course.courseDescription= course.courseDescription== null ? "" : course.courseDescription;

            generateUniqueCourseID(ref course);
        }

        private static void generateUniqueCourseID(ref cours course)
        {
            // At this point we are assuming that the Course object provided will have all upper case.
            StringBuilder uniqueID = new StringBuilder("CRS-");

            uniqueID.Append(course.semester);
            uniqueID.Append(course.year);
            string[] username = course.professorID.Split('@');
            uniqueID.Append(username[0]);

            uniqueID.Append(course.startTime.Hours);
            uniqueID.Append(course.endTime.Minutes);
            uniqueID.Append(course.days);

            course.courseID = uniqueID.ToString();

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