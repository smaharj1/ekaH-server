using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    /// <summary>
    /// This class handles all the requests related to assignments.
    /// </summary>
    public class AssignmentController : ApiController
    {
        /// <summary>
        /// It holds the entity for connection and manipulation of the database.
        /// </summary>
        private ekahEntities11 m_db = new ekahEntities11();

        /// <summary>
        /// GET: ekah/assignments/{courses}/{id}
        /// This function gets all the assignments of a certain course
        /// </summary>
        /// <param name="id">It holds the course id.</param>
        /// <returns>Returns all the assignments of a course.</returns>
        [HttpGet]
        [ActionName("courses")]
        public IHttpActionResult GetAllAssignmentsByCourse(string id)
        {
            List<assignment> assignments = m_db.assignments.Where(a => a.courseID == id).ToList();

            if (assignments.Count == 0)
            {
                return NotFound();
            }

            return Ok(assignments);
        }

        /// <summary>
        /// POST: Makes a post call to save the assignment to the database.
        /// </summary>
        /// <param name="a_assgn">It holds the assignment.</param>
        /// <returns>Returns the result after posting.</returns>
        [HttpPost]
        [ActionName("courses")]
        public IHttpActionResult PostAssignmentByCourse([FromBody] assignment a_assgn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (assignmentExists(a_assgn.courseID, a_assgn.projectNum))
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            /// Adds the assignment to the database.
            assignment updated = m_db.assignments.Add(a_assgn);
            
            try
            {
                m_db.SaveChanges();
            }
            catch(DbUpdateException)
            {
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// This function changes the existing courses values according to the id.
        /// </summary>
        /// <param name="id">It holds the assignment id.</param>
        /// <param name="a_assgn">It holds the assignment that needs to be modified.</param>
        /// <returns>Returns the result of modification.</returns>
        [HttpPut]
        [ActionName("courses")]
        public IHttpActionResult PutAssignmentByCourse(string id, [FromBody] assignment a_assgn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /// Modifies the entry in the database.
            m_db.Entry(a_assgn).State = EntityState.Modified;

            try
            {
                m_db.SaveChanges();
            }
            catch(DbUpdateException)
            {
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// This function checks if the assignment already exists in the database.
        /// </summary>
        /// <param name="a_courseID">Holds the course id.</param>
        /// <param name="a_projectNum">Holds the project number.</param>
        /// <returns>Returns true if the assignment exists.</returns>
        private bool assignmentExists(string a_courseID, int a_projectNum)
        {
            return m_db.assignments.Count(assgn => assgn.courseID == a_courseID && assgn.projectNum == a_projectNum) > 0;
        }
    }
}
