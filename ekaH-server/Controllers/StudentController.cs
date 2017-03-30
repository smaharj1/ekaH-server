using ekaH_server.App_DBHandler;
using ekaH_server.Models.UserModels;
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
    public class StudentController : ApiController
    {
        private ekahEntities11 db = new ekahEntities11();

        // GET: api/Student/{id}
        // Returns the studentinfo object
        public IHttpActionResult Get(string id)
        {
            student_info student = db.student_info.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // POST: api/Student
        // Since we already will have the students registered, we won't need post request here.
        // This is handled by PUT.
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Student/email@email.com
        public IHttpActionResult Put(string id, [FromBody]student_info student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.email)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!studentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
            
        }

        // DELETE: api/Student/5
        public void Delete(int id)
        {
        }

        private bool studentExists(string id)
        {
            return db.student_info.Count(e => e.email == id) > 0;
        }
        

    }
}
