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
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace ekaH_server.Controllers
{
    public class CourseController : ApiController
    {
        private ekahEntities11 db = new ekahEntities11();

        // GET: "ekah/courses/{cid}/{action}
        // It returns the details of a single course by the given id
        [HttpGet]
        [ActionName("single")]
        public IHttpActionResult Get(string cid)
        {
            cours course = db.courses.Find(cid);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
            
        }


        // GET: "Ekah/courses/{cid}/{action}
        // Returns the list of students enrolled in a course.
        [HttpGet]
        [ActionName("students")]
        public IHttpActionResult GetStudentsInCourse(string cid)
        {
            List<studentcourse> allRecords = db.studentcourses.Where(e => e.courseID == cid).ToList();
            
            if (allRecords.Count == 0)
            {
                return NotFound();
            }

            return Ok(allRecords);

            

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

            List<cours> allCoursesByFaculty = db.courses.Where(entry => entry.professorID == cid).ToList();

            if (allCoursesByFaculty == null)
            {
                return NotFound();
            }

            return Ok(allCoursesByFaculty);
            
        }

        // POST: "ekah/courses/{id}
        // This methods helps create a course for certain professor.
        public IHttpActionResult Post([FromBody]cours course)
        {
            if (!(new EmailAddressAttribute().IsValid(course.professorID)))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (courseExists(course.courseID))
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            Course.fixCourseObject(ref course);
            db.courses.Add(course);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return InternalServerError();
            }

            return Ok();
            
        }

        // PUT: "ekah/courses/{id}
        // Helps change the information of the faculty for the courses they have already added.
        [HttpPut]
        public IHttpActionResult Put([FromBody]cours course)
        {
            // TODO: Modify is currently under review since primary key is course ID that we generated.
            if (!(new EmailAddressAttribute().IsValid(course.professorID)))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.Delete(course.courseID);
            Course.fixCourseObject(ref course);
            // The id for this course is generated through an algorithm. So, it cannot be directly modified.
            //db.Entry(course).State = EntityState.Modified;
            db.courses.Add(course);
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!courseExists(course.courseID))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }

            return Ok();

            /*
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
            */

        }

        // DELETE: ekah/courses/{cid}
        // It deletes the course from the database. 
        // It does need two parameters for ids since delete does not take in any body while following strict http protocol.
        public IHttpActionResult Delete(string cid)
        {
            if (!courseExists(cid))
            {
                return NotFound();
            }

            cours course = db.courses.Find(cid);
            db.courses.Remove(course);

            db.SaveChanges();

            return Ok();

        }

        private bool courseExists(string cid)
        {
            return db.courses.Count(e => e.courseID == cid) > 0;

        }
    }
}
