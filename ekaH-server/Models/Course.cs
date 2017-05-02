using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections;
using ekaH_server.App_DBHandler;

namespace ekaH_server.Models
{
    /// <summary>
    /// This class handles various functionalities to fix and generate course ids.
    /// </summary>
    public class Course 
    {
        /// <summary>
        /// This function fixes the course object according the rules.
        /// </summary>
        /// <param name="a_course">It holds the course object.</param>
        public static void FixCourseObject(ref cours a_course)
        {
            /// Some of them would be strict. So, they don't need fixing like year.
            a_course.semester = a_course.semester.ToUpper();
            a_course.days = a_course.days.ToUpper();

            a_course.courseName= a_course.courseName == null ? "" : a_course.courseName;
            a_course.courseDescription= a_course.courseDescription== null ? "" : a_course.courseDescription;

            /// It generates a unique id from the course details provided.
            generateUniqueCourseID(ref a_course);
        }

        /// <summary>
        /// This function generates a unique course id.
        /// </summary>
        /// <param name="a_course">It holds the course data structure.</param>
        private static void generateUniqueCourseID(ref cours a_course)
        {
            /// At this point we are assuming that the Course object provided will have all upper case.
            StringBuilder uniqueID = new StringBuilder("CRS-");

            uniqueID.Append(a_course.semester);
            uniqueID.Append(a_course.year);
            string[] username = a_course.professorID.Split('@');
            uniqueID.Append(username[0]);

            uniqueID.Append(a_course.startTime.Hours);
            uniqueID.Append(a_course.endTime.Minutes);
            uniqueID.Append(a_course.days);

            a_course.courseID = uniqueID.ToString();
        }
    }
}