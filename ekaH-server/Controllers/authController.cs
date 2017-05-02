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
            return null;
        }

        /// <summary>
        /// POST: api/ekaH/login
        /// This function handles if the user tries to log in. If it is true, it should return true. Else,
        /// return the error message.
        /// </summary>
        /// <param name="a_providedInfo">It holds the information provided by the user.</param>
        /// <returns>Returns if the login information provided is correct.</returns>
        [ActionName("login")]
        public IHttpActionResult Post([FromBody] authentication a_providedInfo)
        {
            authentication result = db.authentications.Find(a_providedInfo.email);

            if (result != null)
            {
                /// Validates the hashed password.
                if (Hashing.ValidatePassword(a_providedInfo.pswd, result.pswd))
                {
                    /// If it is student, the data should be in student profile as well.
                    if (a_providedInfo.member_type == result.member_type) return Ok();
                    else return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.Ambiguous);
                }
                
            }

            return NotFound();
        }

        /// <summary>
        /// This function registers the user to the database for the application.
        /// </summary>
        /// <param name="a_providedInfo">It holds the information provided by the user.</param>
        /// <returns>Returns ok if the user is registered.</returns>
        [HttpPost]
        [ActionName("register")]
        public IHttpActionResult registerUser([FromBody] RegisterInfo a_providedInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isStudent = a_providedInfo.isStudent;

            /// Checks if the user already exists in the database.
            if (userExists(a_providedInfo.userEmail))
            {
                return Conflict();
            }

            /// Hashes the password.
            authentication authTemp = new authentication();
            authTemp.email = a_providedInfo.userEmail;
            authTemp.member_type = (sbyte)(a_providedInfo.isStudent ? 1 : 0);
            
            authTemp.pswd = Hashing.HashPassword(a_providedInfo.pswd);

            db.authentications.Add(authTemp);
            
            /// Stores the information in the designated table for students/faculty.
            if (isStudent)
            {
                student_info student = new student_info();
                student.firstName = a_providedInfo.firstName;
                student.lastName = a_providedInfo.lastName;
                student.email = a_providedInfo.userEmail;
                student.graduationYear = int.Parse(a_providedInfo.extraInfo);

                db.student_info.Add(student);
            }
            else
            {
                professor_info professor = new professor_info();
                professor.firstName = a_providedInfo.firstName;
                professor.lastName = a_providedInfo.lastName;
                professor.email = a_providedInfo.userEmail;
                professor.department = a_providedInfo.extraInfo;
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

        /// <summary>
        /// This function checks if the user exists.
        /// </summary>
        /// <param name="a_id">It holds the id of the user.</param>
        /// <returns>Returns true if the user exists.</returns>
        private bool userExists(string a_id)
        {
            return db.authentications.Count(e => e.email == a_id) > 0;
        }
    }
}
