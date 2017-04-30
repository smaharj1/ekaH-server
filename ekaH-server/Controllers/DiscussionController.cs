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
        private ekahEntities11 db = new ekahEntities11();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">It represents the assignment id.</param>
        /// <returns></returns>
        [HttpGet] 
        [ActionName("thread")]
        public IHttpActionResult GetDiscussion(int id)
        {
            List<discussion> threads = db.discussions.Where(diss => diss.assignmentID == id).ToList();

            if (threads.Count > 0) return Ok(threads[0]);
            else return NotFound();
        }

        [HttpPost]
        [ActionName("thread")]
        public IHttpActionResult PostDiscussion([FromBody] discussion conv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (DiscussionExists(conv.assignmentID))
            {
                return Conflict();
            }

            db.discussions.Add(conv);

            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPut]
        [ActionName("thread")]
        public IHttpActionResult PutDiscussion(int id, [FromBody] discussion conv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!DiscussionExists(conv.assignmentID))
            {
                PostDiscussion(conv);
                return Ok();
            }

            db.Entry(conv).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return InternalServerError();
            }

            return Ok();
        }

        private Boolean DiscussionExists(long? assgnId)
        {
            return db.discussions.Count(diss => diss.assignmentID == assgnId) > 0;
        }
    }
}
