using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ekaH_server.Models;
using ekaH_server.App_DBHandler;
using ekaH_server.Models.UserAuth;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace ekaH_server.Controllers
{
    public class authController : ApiController
    {
        ekahEntities11 db = new ekahEntities11();

        // GET: api/ekaH
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ekaH/Get/5
        public LogInInfo Get(int id)
        {
            //LogInInfo linfo = new LogInInfo("smaharj1@ramapo.edu", "asdf", true);
            return null;
        }

        // GET: api/ekaH/person

        /*
         * POST: api/ekaH/login
         * It handles if the user tries to log in. If it is true, it should return true. Else,
         * return the error message.
         * */
        [ActionName("login")]
        public IHttpActionResult Post([FromBody] authentication providedInfo)
        {
            authentication result = db.authentications.Find(providedInfo.email);

            if (result != null)
            {
               
                if (Hashing.ValidatePassword(providedInfo.pswd, result.pswd))
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(HttpStatusCode.Ambiguous);
                }
                
            }

            return NotFound();

            
        }

        [HttpPost]
        [ActionName("register")]
        public IHttpActionResult registerUser([FromBody] RegisterInfo providedInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isStudent = providedInfo.isStudent;

            if (userExists(providedInfo.userEmail))
            {
                return Conflict();
            }
            authentication authTemp = new authentication();
            authTemp.email = providedInfo.userEmail;
            authTemp.member_type = (sbyte)(providedInfo.isStudent ? 1 : 0);
            
            authTemp.pswd = Hashing.HashPassword(providedInfo.pswd);

            db.authentications.Add(authTemp);

            if (isStudent)
            {
                student_info student = new student_info();
                student.firstName = providedInfo.firstName;
                student.lastName = providedInfo.lastName;
                student.email = providedInfo.userEmail;
                student.graduationYear = int.Parse(providedInfo.extraInfo);

                db.student_info.Add(student);
            }
            else
            {
                professor_info professor = new professor_info();
                professor.firstName = providedInfo.firstName;
                professor.lastName = providedInfo.lastName;
                professor.email = providedInfo.userEmail;
                professor.department = providedInfo.extraInfo;
                db.professor_info.Add(professor);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }

        // PUT: api/ekaH/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ekaH/5
        public void Delete(int id)
        {
        }

        private bool userExists(string id)
        {
            return db.authentications.Count(e => e.email == id) > 0;
        }
    }
}
