using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using ekaH_server.Models.UserModels;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ekaH_server.Controllers
{
    public class facultyController : ApiController
    {
        private ekahEntities11 db = new ekahEntities11();

        // GET: ekah/faculties/{id}
        // Returns the information of the user from user info database. 
        // Todo: Also returns the next thing in the list. Either courses or appointments.
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            professor_info professor = db.professor_info.Find(id);

            if (professor == null)
            {
                return NotFound();
            }

            return Ok(professor);

            /*
            IHttpActionResult result;

            bool isStudent = true; 
            try
            {
                isStudent = UserAuthentication.getUserType(id);
            }
            catch(Exception)
            {
                return InternalServerError();
            }

            if (isStudent) return BadRequest();

            try
            {
                FacultyInfo faculty = FacultyDBHandler.executeFacultyInfoQuery(id);

                if (faculty == null)
                {
                    result = NotFound();
                }
                else
                {
                    result = Ok(faculty);
                }
            }
            catch (Exception)
            {
                result = InternalServerError();
            }
                
            

            return result;*/
        }

        // POST: api/faculties/{id}
        // Posts the information updated like name and things. We don't need this currently since PUT does its work.
        public void Post([FromBody]string value)
        {
        }

        // PUT: ekah/faculties/{id}
        [HttpPut]
        public IHttpActionResult Put(string id, [FromBody] professor_info professor)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != professor.email)
            {
                return BadRequest();
            }

            db.Entry(professor).State = EntityState.Modified; 

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (!professorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);

            /*
            IHttpActionResult response;

            bool isStudent = true;
            try
            {
                isStudent = UserAuthentication.getUserType(id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            if (isStudent) return BadRequest();

            FacultyInfo faculty = professor;
            faculty.Email = id;
            bool result = FacultyDBHandler.executePutFacultyInfo(faculty);

            if (result)
            {
                // Return true since the table has been updated. Else, return false because there has been an error.
                response =  Ok();
            }
            else
            {
                // The error indicates that the database found an error.
                response = InternalServerError();
            }
            
            return response;*/
        }

        private bool professorExists(string id)
        {
            return db.professor_info.Count(e => e.email == id) > 0;
        }


    }
}
