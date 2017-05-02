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
    public class StudentController : ApiController
    {
        /// <summary>
        /// It holds the entity for db connection
        /// </summary>
        private ekahEntities11 m_db = new ekahEntities11();

        /// <summary>
        /// GET: api/Student/{id}
        /// This function returns the studentinfo object.
        /// </summary>
        /// <param name="id">It holds student's email.</param>
        /// <returns>Returns the information of student.</returns>
        public IHttpActionResult Get(string id)
        {
            student_info student = m_db.student_info.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Student/email@email.com
        /// <summary>
        /// This function modifies the current user information.
        /// </summary>
        /// <param name="id">It holds the email address of the student.</param>
        /// <param name="a_student">It holds the modified values of the student.</param>
        /// <returns>Returns the status of modification.</returns>
        public IHttpActionResult Put(string id, [FromBody]student_info a_student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != a_student.email)
            {
                return BadRequest();
            }

            /// Modifies the values.
            m_db.Entry(a_student).State = EntityState.Modified;

            try
            {
                m_db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!StudentExists(id))
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
        
        /// <summary>
        /// This function checks if the student exists in the database.
        /// </summary>
        /// <param name="a_email">It holds the email address of student.</param>
        /// <returns>Returns true if student exists.</returns>
        private bool StudentExists(string a_email)
        {
            return m_db.student_info.Count(e => e.email == a_email) > 0;
        }
        

    }
}
