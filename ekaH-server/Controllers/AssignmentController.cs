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
    public class AssignmentController : ApiController
    {
        private ekahEntities11 db = new ekahEntities11();

        /// GET: ekah/assignments/{courses}/{id}
        /// Gets all the assignments of a certain course
        [HttpGet]
        [ActionName("courses")]
        public IHttpActionResult GetAllAssignmentsByCourse(string id)
        {
            List<assignment> assignments = db.assignments.Where(a => a.courseID == id).ToList();

            if (assignments.Count == 0)
            {
                return NotFound();
            }

            return Ok(assignments);
        }

        /// <summary>
        /// POST: Makes a post call to save the assignment to the database.
        /// </summary>
        /// <param name="assgn"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("courses")]
        public IHttpActionResult PostAssignmentByCourse([FromBody] assignment assgn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (assignmentExists(assgn.courseID, assgn.projectNum))
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            db.assignments.Add(assgn);

            try
            {
                db.SaveChanges();
            }
            catch(DbUpdateException)
            {
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// Changes the existing courses values according to the id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assgn"></param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("courses")]
        public IHttpActionResult PutAssignmentByCourse(string id, [FromBody] assignment assgn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(assgn).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch(DbUpdateException)
            {
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// Checks if the assignment already exists in the database.
        /// </summary>
        /// <param name="courseID">Holds the course id.</param>
        /// <param name="projectNum">Holds the project number.</param>
        /// <returns></returns>
        private bool assignmentExists(string courseID, int projectNum)
        {
            return db.assignments.Count(assgn => assgn.courseID == courseID && assgn.projectNum == projectNum) > 0;
        }
    }
}
