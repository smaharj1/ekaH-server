using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using ekaH_server.Models;
using System.Collections;
using ekaH_server.Models.UserModels;

namespace ekaH_server.Controllers
{
    public class CourseController : ApiController
    {
        // GET: "ekah/courses
        public string Get()
        {
            return "No one is supposed to access this";
        }

        // GET: "ekah/courses/{id}
        // Handles the response for faculty and students here. They are not different as both of them should receive 
        // dictionary of all the courses they are teaching or they are studying.
        public IHttpActionResult Get(string id)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }

            

            DBConnection database = DBConnection.getInstance();

            ArrayList allCourses = new ArrayList();
            bool isStudent = true;

            try
            {
                isStudent = UserAuthentication.getUserType(database, id);
            }
            catch (Exception)
            {
                return NotFound();
            }

                
               
            if (isStudent)
            {
                    
            }
            else
            {
                // This is for faculty. Returns the list of all the courses taught by a faculty member.
                // This returns null if there is SQL ERROR.
                MySqlDataReader reader = FacultyDBHandler.readCoursesByFaculty(id);

                if (reader != null)
                {
                    try
                    {
                        allCourses = Course.normalizeCourses(reader);
                    }
                    catch(Exception)
                    {
                        // This exception helps dispose reader.
                        reader.Dispose();
                    }
                }
                else
                {
                    // This means that there was SQL error.
                    return InternalServerError();
                }
                    
            }
            
            

            //reader.Dispose();
            return Ok(allCourses);
        }

        // POST: "ekah/courses/{id}
        public void Post([FromBody]string value)
        {
        }

        // PUT: "ekah/courses/{id}
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: "ekah/courses/{id}
        public void Delete(int id)
        {
        }
    }
}
