using ekaH_server.App_DBHandler;
using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    public class SubmissionsController : ApiController
    {
        // GET: ekah/submissions/student/id
        // Gets all assignment solutions submitted by student
        [HttpGet]
        [ActionName("submit")]
        public IHttpActionResult GetSubmission(int id)
        {
            try
            {
                Submission sub = AssignmentDB.getSubmissionByID(id);
                if (sub != null) return Ok(sub);
                else return NotFound();
            }catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST & PUT: ekah/submissions/submit/
        // Posts the submission of the student to the database.
        [HttpPost]
        [ActionName("submit")]
        public IHttpActionResult PostSubmissionByStudent([FromBody] Submission submitted)
        {
            // temp
            string hey = "hey world";
            submitted.RawContent = Encoding.ASCII.GetBytes(hey);
            try
            {
                if (AssignmentDB.storeAssignment(submitted, true))
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(HttpStatusCode.Forbidden);
                }
            }catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Delete: ekah/submissions/student/id
        // Deletes the submission given by submission ID
        [HttpDelete]
        [ActionName("submit")]
        public IHttpActionResult DeleteSubmission(int id)
        {
            try
            {
                AssignmentDB.deleteSubmission(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok();
        }

        // POST & PUT: ekah/submissions/submit/
        // This is for changing the functionalities by professor like Grading.
        [HttpPost]
        [ActionName("faculty")]
        public IHttpActionResult PutSubmissionByFaculty([FromBody] Submission submitted)
        {
            try
            {
                if (AssignmentDB.storeAssignment(submitted, false))
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
