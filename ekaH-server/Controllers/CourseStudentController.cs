using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    public class CourseStudentController : ApiController
    {
        // GET: ekah/courses/{cid}/students
        // Returns all the students in a certain course
        public IHttpActionResult Get(string cid)
        {
            if (!CourseDBHandler.courseExists(cid))
            {
                return NotFound();
            }
            List<string> students;
            try
            {
                students = CourseStudentDBHandler.getAllStudentsFromDB(cid);
            }
            catch(Exception)
            {
                return InternalServerError();
            }

            return Ok<List<string>>(students);
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

            bool success = CourseStudentDBHandler.addStudentsToDB(cid, ids);

            if (success)
            {
                return StatusCode(HttpStatusCode.Created);
            }

            return InternalServerError();

        }

        // PUT: ekah/courses/{cid}/students
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: ekah/courses/{cid}/students/{sid}
        // Returns bad request if email is not valid, returns not found if course id is not found.
        // Delete returns ok even if the student doesn't exist.
        public IHttpActionResult Delete(string cid, string sid)
        {
            if (!(new EmailAddressAttribute().IsValid(sid)))
            {
                return BadRequest();
            }

            if (!CourseDBHandler.courseExists(cid))
            {
                return NotFound();
            }

            bool success =  CourseStudentDBHandler.deleteStudentFromDB(cid, sid);

            if (success) return Ok();

            return InternalServerError();

        }
    }
}
