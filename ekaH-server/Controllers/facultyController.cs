using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ekaH_server.Controllers
{
    public class facultyController : ApiController
    {
        /// <summary>
        /// It holds the entity for db connection
        /// </summary>
        private ekahEntities11 m_db = new ekahEntities11();

        /// <summary>
        /// GET: ekah/faculties/{id}
        /// This function returns the information of the user from user info database. 
        /// </summary>
        /// <param name="id">It holds the email of professor.</param>
        /// <returns>Returns the faculty's information.</returns>
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            professor_info professor = m_db.professor_info.Find(id);

            if (professor == null)
            {
                return NotFound();
            }

            return Ok(professor);
        }

        /// <summary>
        /// PUT: ekah/faculties/{id}
        /// This function modifies the current information of the professor.
        /// </summary>
        /// <param name="id">It holds the email of the professor.</param>
        /// <param name="a_professor"></param>
        /// <returns>Returns the status of modification.</returns>
        [HttpPut]
        public IHttpActionResult Put(string id, [FromBody] professor_info a_professor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != a_professor.email)
            {
                return BadRequest();
            }

            /// Modifies the existing value.
            m_db.Entry(a_professor).State = EntityState.Modified; 

            try
            {
                m_db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (!ProfessorExists(id))
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
        /// This function checks if the professor exists.
        /// </summary>
        /// <param name="a_email">It holds the email of professor.</param>
        /// <returns></returns>
        private bool ProfessorExists(string a_email)
        {
            return m_db.professor_info.Count(e => e.email == a_email) > 0;
        }


    }
}
