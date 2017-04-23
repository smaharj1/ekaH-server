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
        
    }
}