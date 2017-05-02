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
        /// <summary>
        /// It holds the entity for db connection
        /// </summary>
        private ekahEntities11 m_db = new ekahEntities11();

        /// <summary>
        /// GET: ekah/submissions/submit/id
        /// This function gets all assignment solutions submitted.
        /// </summary>
        /// <param name="id">It holds the submission id.</param>
        /// <returns>Returns all the submission for the submission id..</returns>
        [HttpGet]
        [ActionName("submit")]
        public IHttpActionResult GetSubmission(int id)
        {
            submission submission = m_db.submissions.Find(id);

            if (submission == null)
            {
                return NotFound();
            }

            return Ok(submission);
            
        }
        
        /// <summary>
        /// This function gets the submission made by the student for the particular project
        /// </summary>
        /// <param name="aid">It represents the assignment id.</param>
        /// <param name="sid">It represents the student email</param>
        /// <returns>Returns the submission made by a student.</returns>
        [HttpGet]
        [ActionName("submit")]
        public IHttpActionResult GetSubmission(int aid, string sid)
        {
            List<submission> submission = m_db.submissions.Where(sub => sub.assignmentID == aid && sub.studentID == sid).ToList();

            return Ok(submission);
        }

        /// <summary>
        /// This function gets the submissions for an assignment.
        /// </summary>
        /// <param name="id">It holds the assignment id.</param>
        /// <returns>Returns all the submissions for an assignment.</returns>
        [HttpGet]
        [ActionName("assignments")]
        public IHttpActionResult GetSubmissionByAssgnID(int id)
        {
            List<submission> allSubs = m_db.submissions.Where(sub => sub.assignmentID == id).ToList();

            return Ok(allSubs);
        }


        /// <summary>
        /// POST & PUT: ekah/submissions/submit/
        /// This function posts the submission of the student to the database.
        /// </summary>
        /// <param name="a_submitted">It holds the file to be submitted.</param>
        /// <returns>Returns the status after submission.</returns>
        [HttpPost]
        [ActionName("submit")]
        public IHttpActionResult PostSubmissionByStudent([FromBody] submission a_submitted)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            a_submitted.grade = -1;
            assignment assign = m_db.assignments.Find(a_submitted.assignmentID);

            /// Checks if the date is already passed.
            if (assign.deadline < DateTime.Now)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            
            /// Adds the submission to the database.
            m_db.submissions.Add(a_submitted);

            try
            {
                m_db.SaveChanges();
            }
            catch(Exception)
            {
                if (SubmissionExists(a_submitted.id))
                {
                    /// If the submission already exists, it simply modifies it.
                    m_db.Entry(a_submitted).State = EntityState.Modified;

                    m_db.SaveChanges();
                }
                else
                {
                    return InternalServerError();
                }
            }

            return Ok();
            
        }

        /// <summary>
        /// Delete: ekah/submissions/student/id
        /// This function deletes the submission given by submission ID
        /// </summary>
        /// <param name="id">It holds the id of submission.</param>
        /// <returns>Returns the status of deletion.</returns>
        [HttpDelete]
        [ActionName("submit")]
        public IHttpActionResult DeleteSubmission(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            submission sub = m_db.submissions.Find(id);
            if (sub==null)
            {
                return NotFound();
            }

            m_db.submissions.Remove(sub);
            m_db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// PUT: ekah/submissions/faculty/
        /// This function is for changing the functionalities by professor like Grading.
        /// </summary>
        /// <param name="id">It holds the submission id.</param>
        /// <param name="a_submitted">It holds the modified submission.</param>
        /// <returns>Returns the status of modification.</returns>
        [HttpPut]
        [ActionName("faculty")]
        public IHttpActionResult PutSubmissionByFaculty(long id, [FromBody] submission a_submitted)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!SubmissionExists(id))
            {
                return NotFound();
            }

            /// Modifies the submission.
            m_db.Entry(a_submitted).State = EntityState.Modified;

            try
            {
                m_db.SaveChanges();
            }
            catch(Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// This function checks if the submission exists.
        /// </summary>
        /// <param name="id">It holds the id.</param>
        /// <returns>Returns true if the submission already exists.</returns>
        private bool SubmissionExists(long id)
        {
            return m_db.submissions.Count(sub => sub.id == id) > 0;
        }
    }
}
