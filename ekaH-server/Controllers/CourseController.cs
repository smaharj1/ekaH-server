using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using ekaH_server.Models;
using System.Collections;
using System.Windows.Forms;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace ekaH_server.Controllers
{
    /// <summary>
    /// This class handles all the modification/creation functionalities of a course.
    /// </summary>
    public class CourseController : ApiController
    {
        /// <summary>
        /// It holds the entity for the databse.
        /// </summary>
        private ekahEntities11 m_db = new ekahEntities11();

        /// <summary>
        /// GET: "ekah/courses/{cid}/{action}
        /// This function returns the details of a single course by the given id
        /// </summary>
        /// <param name="cid">It holds the course id.</param>
        /// <returns>Returns the course with the given id.</returns>
        [HttpGet]
        [ActionName("single")]
        public IHttpActionResult Get(string cid)
        {
            cours course = m_db.courses.Find(cid);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }


        /// <summary>
        /// GET: "Ekah/courses/{cid}/{action}
        /// This function returns the list of students enrolled in a course.
        /// </summary>
        /// <param name="cid">It holds the course id.</param>
        /// <returns>Returns all the students enrolled in the course.</returns>
        [HttpGet]
        [ActionName("students")]
        public IHttpActionResult GetStudentsInCourse(string cid)
        {
            List<studentcourse> allRecords = m_db.studentcourses.Where(e => e.courseID == cid).ToList();
            
            if (allRecords.Count == 0)
            {
                return NotFound();
            }

            return Ok(allRecords);
        }

        /// <summary>
        /// GET: "ekah/courses/{cid}/{action}
        /// This function gets all the courses 
        /// that the faculty teaches.
        /// </summary>
        /// <param name="cid">It holds the faculty's email address.</param>
        /// <returns>Returns all the courses taught by the faculty.</returns>
        [HttpGet]
        [ActionName("faculty")]
        public IHttpActionResult GetCourseTaughtByFaculty(string cid)
        {
            // Cid actually refers to the faculty email address in this scenario.
            if (!(new EmailAddressAttribute().IsValid(cid)))
            {
                return BadRequest();
            }

            List<cours> allCoursesByFaculty = m_db.courses.Where(entry => entry.professorID == cid).ToList();

            if (allCoursesByFaculty == null)
            {
                return NotFound();
            }

            return Ok(allCoursesByFaculty);
            
        }

        /// <summary>
        /// POST: "ekah/courses/{id}
        /// This methods helps create a course for certain professor.
        /// </summary>
        /// <param name="a_course">It holds the course.</param>
        /// <returns>Returns the result of posting the course.</returns>
        public IHttpActionResult Post([FromBody]cours a_course)
        {
            if (!(new EmailAddressAttribute().IsValid(a_course.professorID)))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /// Checks if the course already exists.
            if (CourseExists(a_course.courseID))
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            /// Fixes the course object before posting it.
            Course.FixCourseObject(ref a_course);
            m_db.courses.Add(a_course);

            try
            {
                m_db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return InternalServerError();
            }

            return Ok();
            
        }

        // 
        /// <summary>
        /// PUT: "ekah/courses/{id}
        /// This function helps change the information of the faculty for the courses they have already added.
        /// </summary>
        /// <param name="a_course">It holds the course.</param>
        /// <returns>Returns the modification value.</returns>
        [HttpPut]
        public IHttpActionResult Put([FromBody]cours a_course)
        {
            if (!(new EmailAddressAttribute().IsValid(a_course.professorID)))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.Delete(a_course.courseID);
            Course.FixCourseObject(ref a_course);

            /// The id for this course is generated through an algorithm. 
            /// So, it cannot be directly modified.
            m_db.courses.Add(a_course);
            
            try
            {
                m_db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (!CourseExists(a_course.courseID))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }

            return Ok();
                        
        }
        
        /// <summary>
        /// DELETE: ekah/courses/{cid}
        /// It deletes the course from the database. 
        /// It does need two parameters for ids since delete does not take in any body while following strict http protocol.
        /// </summary>
        /// <param name="cid">It holds the course id.</param>
        /// <returns>Returns the result after deletion.</returns>
        public IHttpActionResult Delete(string cid)
        {
            if (!CourseExists(cid))
            {
                return NotFound();
            }

            cours course = m_db.courses.Find(cid);
            m_db.courses.Remove(course);

            m_db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// This function gets all the courses according to the given parameters like
        /// email, semester and eyar.
        /// </summary>
        /// <param name="a_email">It holds the email.</param>
        /// <param name="a_semester">It holds the semester to look for.</param>
        /// <param name="a_year">It holds the year to look for.</param>
        /// <returns>Returns the list of courses.</returns>
        public List<cours> GetCoursesByParameters(string a_email, string a_semester, short a_year)
        {
            List<cours> allCoursesByFaculty = m_db.courses.Where(cr => cr.professorID == a_email && cr.semester == a_semester && cr.year == a_year).ToList();

            return allCoursesByFaculty;   
        }

        /// <summary>
        /// This function returns the string representation of courses in the database.
        /// </summary>
        /// <param name="a_email">It holds professor's email.</param>
        /// <param name="a_semester">It holds semester value.</param>
        /// <param name="a_year">It holds the year value.</param>
        /// <returns>Returns the list of strings of course that are user friendly.</returns>
        public List<string> GetCoursesByParametersInString(string a_email, string a_semester, short a_year)
        {
            /// Gets the list of courses.
            List<cours> courses = GetCoursesByParameters(a_email, a_semester, a_year);

            List<string> result = new List<string>();

            /// Parses the course information to user readable format in string.
            foreach(cours course in courses)
            {
                DateTime start = DateTime.Today + course.startTime;
                DateTime end = DateTime.Today + course.endTime;
                result.Add(course.courseName +" "+ course.days +" " + start.ToString("hh:mm tt") + " to " + end.ToString("hh:mm tt"));
            }

            return result;
        }

        /// <summary>
        /// This function checks if the course exists.
        /// </summary>
        /// <param name="a_cid">It holds the course id.</param>
        /// <returns>Returns true if the course exists.</returns>
        private bool CourseExists(string a_cid)
        {
            return m_db.courses.Count(e => e.courseID == a_cid) > 0;

        }
    }
}
