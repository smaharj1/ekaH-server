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

        // GET: "ekah/courses/{cid}/{action}
        // It returns the details of a single course by the given id
        [HttpGet]
        [ActionName("single")]
        public IHttpActionResult Get(string cid)
        {
            Course c;
            try
            {
                c = CourseDBHandler.getCourseByID(cid);
            }
            catch(Exception)
            {
                // Returns the database error when exception is thrown
                return InternalServerError();
            }

            // Checks if course is null. If it is, it means that the course is not found.
            // Else, returns the course.
            if (c == null) return Ok<Course>(null);
            else return Ok<Course>(c);
        }


        // GET: "Ekah/courses/{cid}/{action}
        // Returns the list of students enrolled in a course.
        [HttpGet]
        [ActionName("students")]
        public IHttpActionResult GetStudentsInCourse(string cid)
        {
            try
            {
                List<StudentInfo> students = CourseDBHandler.getAllStudentsByCourse(cid);
                return Ok(students);
            }
            catch(Exception)
            {
                return InternalServerError();
            }

        }

        // GET: "ekah/courses/{cid}/{action}
        // Including one extra functionality for faculty. Get all the courses 
        // that the faculty teaches.
        [HttpGet]
        [ActionName("faculty")]
        public IHttpActionResult GetCourseTaughtByFaculty(string cid)
        {
            // cid actually refers to the faculty email address in this scenario.
            if (!(new EmailAddressAttribute().IsValid(cid)))
            {
                return BadRequest();
            }

            List<Course> allCourses = new List<Course>();
            bool isStudent = true;

            try
            {
                isStudent = UserAuthentication.getUserType(cid);
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (isStudent)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            
            // This is for faculty. Returns the list of all the courses taught by a faculty member.
            // This returns null if there is SQL ERROR.
            MySqlDataReader reader = CourseDBHandler.readCoursesByFaculty(cid);

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
                    
            

            return Ok(allCourses);
        }

        // POST: "ekah/courses/{id}
        // This methods helps create a course for certain professor.
        public IHttpActionResult Post([FromBody]Course course)
        {
            if (!(new EmailAddressAttribute().IsValid(course.ProfessorID)))
            {
                return BadRequest();
            }
            
            bool isStudent = true;

            // Get the type of the student first as professors can only add/remove the course.
            try
            {
                isStudent = UserAuthentication.getUserType(course.ProfessorID);
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (isStudent)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

          
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
                return Ok(course.CourseID);
            }

            return InternalServerError();
        }

        // PUT: "ekah/courses/{id}
        // Helps change the information of the faculty for the courses they have already added.
        public IHttpActionResult Put([FromBody]Course course)
        {
            if (!(new EmailAddressAttribute().IsValid(course.ProfessorID)))
            {
                return BadRequest();
            }

            bool isStudent = true;

            // Get the type of the student first as professors can only add/remove the course.
            try
            {
                isStudent = UserAuthentication.getUserType(course.ProfessorID);
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (isStudent)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

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
                    if (CourseDBHandler.executePutCourse(course, previousCourseID)) return Ok(course.CourseID);
                    else throw new Exception();
                }
                else
                {
                    // Indicates that the course user wants to edit doesn't exist. Hence, redirect it to POST's method.
                    if (CourseDBHandler.executePostCourse(course)) return Created("",course.CourseID);
                    else throw new Exception();
                }
            }
            catch(Exception)
            {
                // This is only thrown for database exception.
                return InternalServerError();
            }

        }

        // DELETE: ekah/courses/{cid}
        // It deletes the course from the database. 
        // It does need two parameters for ids since delete does not take in any body while following strict http protocol.
        public IHttpActionResult Delete(string cid)
        {
            try
            {
                if (CourseDBHandler.executeDeleteCourse(cid))
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            return InternalServerError();
        }

        

        
        

       
    }
}
