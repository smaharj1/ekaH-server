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
    public class DiscussionController : ApiController
    {
        /// <summary>
        /// It holds the entity for db connection
        /// </summary>
        private ekahEntities11 m_db = new ekahEntities11();

        /// <summary>
        /// This function gets the discussion for the id.
        /// </summary>
        /// <param name="id">It represents the assignment id.</param>
        /// <returns>Returns the discussion.</returns>
        [HttpGet] 
        [ActionName("thread")]
        public IHttpActionResult GetDiscussion(int id)
        {
            /// Gets all the discussion.
            List<discussion> threads = m_db.discussions.Where(diss => diss.assignmentID == id).ToList();

            if (threads.Count > 0) return Ok(threads[0]);
            else return NotFound();
        }

        /// <summary>
        /// This function saves a discussion in to the database.
        /// </summary>
        /// <param name="a_conv">It holds the discussion value.</param>
        /// <returns>Returns the result after posting.</returns>
        [HttpPost]
        [ActionName("thread")]
        public IHttpActionResult PostDiscussion([FromBody] discussion a_conv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /// Checks if the discussion exists first because multiple values for
            /// same discussion aren't allowed.
            if (DiscussionExists(a_conv.assignmentID))
            {
                return Conflict();
            }

            /// Adds the discussion.
            m_db.discussions.Add(a_conv);

            try
            {
                m_db.SaveChanges();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.Created);
        }

        /// <summary>
        /// This function modifies the content of existing discussion.
        /// </summary>
        /// <param name="id">It holds the discussion id.</param>
        /// <param name="a_conv">It holds the modifying discussion.</param>
        /// <returns>Returns the status of modification.</returns>
        [HttpPut]
        [ActionName("thread")]
        public IHttpActionResult PutDiscussion(int id, [FromBody] discussion a_conv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!DiscussionExists(a_conv.assignmentID))
            {
                PostDiscussion(a_conv);
                return Ok();
            }

            /// Modifies the content.
            m_db.Entry(a_conv).State = EntityState.Modified;

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

        /// <summary>
        /// This function checks if discussion exists.
        /// </summary>
        /// <param name="a_assgnId">It holds the assignment id.</param>
        /// <returns>Returns true if discussion already exists.</returns>
        private bool DiscussionExists(long? a_assgnId)
        {
            return m_db.discussions.Count(diss => diss.assignmentID == a_assgnId) > 0;
        }
    }
}
