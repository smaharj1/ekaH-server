using ekaH_server.App_DBHandler;
using ekaH_server.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    public class StudentController : ApiController
    {
        // GET: api/Student
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Student/{id}
        // Returns the studentinfo object
        public IHttpActionResult Get(string id)
        {
            bool isStudent = true;
            try
            {
                isStudent = UserAuthentication.getUserType(id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            if (!isStudent) return BadRequest();

            // Handle the request for student here.

            IHttpActionResult result;
            try
            {
                StudentInfo student = FacultyDBHandler.executeStudentInfoQuery(id);

                if (student == null)
                {
                    result = NotFound();
                }
                else
                {
                    result = Ok(student);
                }
            }
            catch (Exception)
            {
                result = InternalServerError();
            }

            return result;
        }

        // POST: api/Student
        // Since we already will have the students registered, we won't need post request here.
        // This is handled by PUT.
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Student/5
        public IHttpActionResult Put(string id, [FromBody]StudentInfo student)
        {
            bool isStudent = true;
            try
            {
                isStudent = UserAuthentication.getUserType(id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            if (!isStudent) return BadRequest();

            IHttpActionResult response;
            student.Email = id;
            bool result = FacultyDBHandler.executePutStudentInfo(student);

            if (result)
            {
                // Return true since the table has been updated. Else, return false because there has been an error.
                response = Ok();
            }
            else
            {
                // The error indicates that the database found an error.
                response = InternalServerError();
            }

            return response;
        }

        // DELETE: api/Student/5
        public void Delete(int id)
        {
        }
    }
}
