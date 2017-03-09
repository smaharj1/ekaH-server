using ekaH_server.App_DBHandler;
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
    public class StudentCourseController : ApiController
    {
        // GET: ekah/students/{id}/courses
        // Returns all the courses taken byt he student
        public IHttpActionResult Get(string id)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }
            ArrayList courses = new ArrayList();


            try
            {
                courses = StudentCourseDBHandler.getAllCoursesByStudent(id);
            }
            catch(Exception)
            {
                return InternalServerError();
            }

            return Ok(courses);
        }
        

        // POST: ekah/courses/{cid}/students
        public IHttpActionResult Post(string cid, [FromBody]List<string> ids)
        {
            // Assumes that the client should sent correct email addresses in the body.
            // If not, just remove those emails from the list.
            foreach (string email in ids)
            {
                if (!(new EmailAddressAttribute().IsValid(email)))
                {
                    ids.Remove(email);
                }
            }

            if (!CourseDBHandler.courseExists(cid))
            {
                return NotFound();
            }

            bool success = StudentCourseDBHandler.addStudentsToDB(cid, ids);

            if (success)
            {
                return StatusCode(HttpStatusCode.Created);
            }

            return InternalServerError();

        }

        // POST: ekah/students/{id}/courses/{cid}
        // Adds one student to a course at a time.
        public IHttpActionResult Post(string id, string cid)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }

            if (!CourseDBHandler.courseExists(cid))
            {
                return NotFound();
            }

            try
            {
                if (StudentCourseDBHandler.studentAlreadyExists(cid, id))
                {
                    return StatusCode(HttpStatusCode.Conflict);
                }
            }
            catch (Exception) { return InternalServerError(); }

            bool success = StudentCourseDBHandler.addOneStudentToCourse(cid, id);

            if (success) return Ok();
            else return InternalServerError();

        }

        // PUT: ekah/courses/{cid}/students
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: ekah/students/{id}/courses/{cid}
        // Returns bad request if email is not valid, returns not found if course id is not found.
        // Delete returns ok even if the student doesn't exist.
        public IHttpActionResult Delete(string id, string cid)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }

            if (!CourseDBHandler.courseExists(cid))
            {
                return NotFound();
            }

            bool success =  StudentCourseDBHandler.deleteStudentFromCourse(cid, id);

            if (success) return Ok();

            return InternalServerError();

        }
    }
}
