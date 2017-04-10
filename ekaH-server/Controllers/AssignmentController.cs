using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    public class AssignmentController : ApiController
    {
        private ekahEntities11 db = new ekahEntities11();

        // GET: ekah/assignments/{courses}/{id}
        // Gets all the assignments of a certain course
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
    }
}
