using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using ekaH_server.Models.UserModels;

namespace ekaH_server.Controllers
{
    public class facultyController : ApiController
    {
        // GET: ekah/faculties
        // Returns all the faculty members
        public IEnumerable<string> Get()
        {
            return new string[] { "No one is authorized for this" };
        }

        // GET: ekah/faculties/{id}
        // Returns the information of the user from user info database. 
        // Todo: Also returns the next thing in the list. Either courses or appointments.
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            IHttpActionResult result;

            MySqlDataReader dataReader = FacultyDBHandler.executeFacultyInfoQuery(id);

            // Data reader not null means that the query has been successfully executed.
            if (dataReader != null)
            {
                FacultyInfo facultyMem = FacultyWorker.extractInfo(dataReader);

                if (facultyMem == null)
                {
                    result = NotFound();
                }
                else
                {
                    result =  Ok(facultyMem);
                }
            }
            else
            {
                result = InternalServerError();
            }

            dataReader.Close();

            return result;
            
        }

        // POST: api/faculties/{id}
        // Posts the information updated like name and things.
        public void Post([FromBody]string value)
        {
        }

        // PUT: ekah/faculties/{id}
        [HttpPut]
        public IHttpActionResult Put(string id, [FromBody] FacultyInfo facultyMem)
        {
            facultyMem.Email = id;
            bool result = FacultyDBHandler.executePutFacultyInfo(facultyMem);

            if (result)
            {
                // Return true since the table has been updated. Else, return false because there has been an error.
                return Ok();
            }
            else
            {
                // The error indicates that the database found an error.
                return InternalServerError();
            }
        }

        // DELETE: api/user/5
        public void Delete(int id)
        {
        }
    }
}
