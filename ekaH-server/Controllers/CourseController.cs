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
using System.Windows.Forms;

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

            

           // DBConnection database = DBConnection.getInstance();

            ArrayList allCourses = new ArrayList();
            bool isStudent = true;

            try
            {
                isStudent = UserAuthentication.getUserType(id);
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
                MySqlDataReader reader = CourseDBHandler.readCoursesByFaculty(id);

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

            return Ok(allCourses);
        }

        // POST: "ekah/courses/{id}
        // This methods helps create a course for certain professor.
        public IHttpActionResult Post(string id, [FromBody]Course course)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }
            
            bool isStudent = true;

            // Get the type of the student first as professors can only add/remove the course.
            try
            {
                isStudent = UserAuthentication.getUserType(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (isStudent)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            course.ProfessorID = id;

            // Handle the situation here since we are sure that it is a faculty member.
            // This is because only faculty members can add the courses.

            if (!course.validateFields())
            {
                return BadRequest();
            }

            Course.fixCourseObject(ref course);

            bool status = CourseDBHandler.executePostCourse(course);

            if (status)
            {
                return StatusCode(HttpStatusCode.Created);
            }

            return InternalServerError();
        }

        // PUT: "ekah/courses/{id}
        // Helps change the information of the faculty for the courses they have already added.
        public IHttpActionResult Put(string id, [FromBody]Course course)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }

            bool isStudent = true;

            // Get the type of the student first as professors can only add/remove the course.
            try
            {
                isStudent = UserAuthentication.getUserType(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (isStudent)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            course.ProfessorID = id;

            // Handle the situation here since we are sure that it is a faculty member.
            // This is because only faculty members can add the courses.

            if (!course.validateFields())
            {
                return BadRequest();
            }

            string previousCourseID = course.CourseID;
            Course.fixCourseObject(ref course);

            try
            {
                if (CourseDBHandler.courseExists(previousCourseID))
                {
                    if (CourseDBHandler.executePutCourse(course, previousCourseID)) return StatusCode(HttpStatusCode.OK);
                    else throw new Exception();
                }
                else
                {
                    // Indicates that the course user wants to edit doesn't exist. Hence, redirect it to POST's method.
                    if (CourseDBHandler.executePostCourse(course)) return StatusCode(HttpStatusCode.Created);
                    else throw new Exception();
                }
            }
            catch(Exception)
            {
                // This is only thrown for database exception.
                return InternalServerError();
            }

        }

        // DELETE: "ekah/courses/{id}
        public void Delete(int id)
        {
        }
    }
}
