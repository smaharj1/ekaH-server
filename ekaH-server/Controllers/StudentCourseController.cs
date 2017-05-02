using ekaH_server.App_DBHandler;
using ekaH_server.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    /// <summary>
    /// It is a controller for handling the courses functions for students. 
    /// For example, get all the courses taken by the students, add course and drop course for students.
    /// </summary>
    public class StudentCourseController : ApiController
    {
        /// <summary>
        /// It holds the entity for db connection
        /// </summary>
        private ekahEntities11 m_db = new ekahEntities11();

        /// <summary>
        /// GET: ekah/students/{id}/courses
        /// This function returns all the courses taken by the student.
        /// </summary>
        /// <param name="id">It holds the email of the student.</param>
        /// <returns>Returns all the courses taken by the student.</returns>
        public IHttpActionResult Get(string id)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }
            student_info student = m_db.student_info.Find(id);

            /// Gets all the courses taken by the student.
            List<studentcourse> studentCour = student.studentcourses.ToList();

            if (studentCour == null) { return NotFound(); }

            List<cours> courses = new List<cours>();
            foreach( studentcourse sc in studentCour)
            {
                courses.Add(sc.cours);
            }

            /// Returns the courses.
            return Ok(courses);
            
        }

        /// <summary>
        /// POST: ekah/students/{id}/courses/{cid}
        /// This function adds one student to a course at a time.
        /// </summary>
        /// <param name="id">It holds the student id.</param>
        /// <param name="cid">It holds the course id.</param>
        /// <returns>Returns the status after adding.</returns>
        public IHttpActionResult Post(string id, string cid)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!CourseExists(cid))
            {
                return NotFound();
            }

            /// Gets the courses by the student to check if the student is already enrolled.
            List<studentcourse> stdCourse = m_db.studentcourses.Where(sc=> sc.courseID == cid && sc.studentID == id).ToList();

            if (stdCourse.Count != 0)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            studentcourse sCourse = new studentcourse();
            sCourse.courseID = cid;
            sCourse.studentID = id;

            /// Adds the student to the course.
            m_db.studentcourses.Add(sCourse);

            try
            {
                m_db.SaveChanges();
            }
            catch(Exception )
            {
                return InternalServerError();
            }

            return Ok();
            
        }

        /// <summary>
        /// DELETE: ekah/students/{id}/courses/{cid}
        /// This function returns bad request if email is not valid, returns not found if course id is not found.
        /// Delete returns ok even if the student doesn't exist.
        /// </summary>
        /// <param name="id">It holds the email id.</param>
        /// <param name="cid">It holds the course id.</param>
        /// <returns>Returns the status of deletion.</returns>
        public IHttpActionResult Delete(string id, string cid)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }

            if (!CourseExists(cid))
            {
                return NotFound();
            }

            List<studentcourse> found = m_db.studentcourses.Where(sc => sc.courseID == cid && sc.studentID == id).ToList();

            if (found.Count == 0 )
            {
                return NotFound();
            }

            /// Removes the student from the course.
            m_db.studentcourses.Remove(found[0]);
            m_db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// This function checks if the course exists in the database.
        /// </summary>
        /// <param name="a_cid">It represents the course id.</param>
        /// <returns>Returns true if course exists.</returns>
        private bool CourseExists(string a_cid)
        {
            return m_db.courses.Count(e => e.courseID == a_cid) > 0;
        }
    }
}
