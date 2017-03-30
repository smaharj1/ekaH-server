using ekaH_server.App_DBHandler;
using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    public class SubmissionsController : ApiController
    {
        private ekahEntities11 db = new ekahEntities11();

        // GET: ekah/submissions/student/id
        // Gets all assignment solutions submitted by student
        [HttpGet]
        [ActionName("submit")]
        public IHttpActionResult GetSubmission(int id)
        {
            submission submission = db.submissions.Find(id);

            if (submission == null)
            {
                return NotFound();
            }

            return Ok(submission);

            /*
            try
            {
                Submission sub = AssignmentDB.getSubmissionByID(id);
                if (sub != null) return Ok(sub);
                else return NotFound();
            }catch(Exception ex)
            {
                return InternalServerError(ex);
            }*/
        }

        // POST & PUT: ekah/submissions/submit/
        // Posts the submission of the student to the database.
        [HttpPost]
        [ActionName("submit")]
        public IHttpActionResult PostSubmissionByStudent([FromBody] submission submitted)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            submitted.grade = -1;
            assignment assign = db.assignments.Find(submitted.assignmentID);

            // Checks if the date is already passed.
            if (assign.deadline < DateTime.Now)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            
            db.submissions.Add(submitted);

            try
            {
                db.SaveChanges();
            }
            catch(Exception)
            {
                if (submissionExists(submitted.id))
                {
                    db.Entry(submitted).State = EntityState.Modified;

                    db.SaveChanges();
                }
                else
                {
                    return InternalServerError();
                }
            }

            return Ok();

            /*
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
            }*/
        }

        // Delete: ekah/submissions/student/id
        // Deletes the submission given by submission ID
        [HttpDelete]
        [ActionName("submit")]
        public IHttpActionResult DeleteSubmission(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            submission sub = db.submissions.Find(id);
            if (sub==null)
            {
                return NotFound();
            }

            db.submissions.Remove(sub);
            db.SaveChanges();

            return Ok();
            /*
            try
            {
                AssignmentDB.deleteSubmission(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok();*/
        }

        // PUT: ekah/submissions/faculty/
        // This is for changing the functionalities by professor like Grading.
        [HttpPost]
        [ActionName("faculty")]
        public IHttpActionResult PutSubmissionByFaculty(long id, [FromBody] submission submitted)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!submissionExists(id))
            {
                return NotFound();
            }

            db.Entry(submitted).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch(Exception)
            {
                return InternalServerError();
            }

            return Ok();

            /*
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
            }*/
        }

        private bool submissionExists(long id)
        {
            return db.submissions.Count(sub => sub.id == id) > 0;
        }
    }
}
