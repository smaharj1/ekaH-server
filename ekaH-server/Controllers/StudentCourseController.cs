using ekaH_server.App_DBHandler;
using ekaH_server.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    // It is a controller for handling the courses functions for students. 
    // For example, get all the courses taken by the students, add course and drop course for students.
    public class StudentCourseController : ApiController
    {
        private ekahEntities11 db = new ekahEntities11();

        // GET: ekah/students/{id}/courses
        // Returns all the courses taken by the student
        public IHttpActionResult Get(string id)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }
            student_info student = db.student_info.Find(id);

            List<studentcourse> studentCour = student.studentcourses.ToList();

            if (studentCour == null) { return NotFound(); }

            List<cours> courses = new List<cours>();
            foreach( studentcourse sc in studentCour)
            {
                courses.Add(sc.cours);
            }

            return Ok(courses);
            
        }
              

        // POST: ekah/students/{id}/courses/{cid}
        // Adds one student to a course at a time.
        public IHttpActionResult Post(string id, string cid)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!courseExists(cid))
            {
                return NotFound();
            }

            
            List<studentcourse> stdCourse = db.studentcourses.Where(sc=> sc.courseID == cid && sc.studentID == id).ToList();

            if (stdCourse.Count != 0)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            studentcourse sCourse = new studentcourse();
            sCourse.courseID = cid;
            sCourse.studentID = id;

            db.studentcourses.Add(sCourse);

            try
            {
                db.SaveChanges();
            }
            catch(Exception )
            {
                return InternalServerError();
            }

            return Ok();
            
        }

        // PUT: ekah/courses/{cid}/students
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: ekah/students/{id}/courses/{cid}
        // Returns bad request if email is not valid, returns not found if course id is not found.
        // Delete returns ok even if the student doesn't exist.
        public IHttpActionResult Delete(string id, string cid)
        {
            if (!(new EmailAddressAttribute().IsValid(id)))
            {
                return BadRequest();
            }

            if (!courseExists(cid))
            {
                return NotFound();
            }

            List<studentcourse> found = db.studentcourses.Where(sc => sc.courseID == cid && sc.studentID == id).ToList();

            if (found.Count == 0 )
            {
                return NotFound();
            }

            db.studentcourses.Remove(found[0]);
            db.SaveChanges();

            return Ok();
            
        }

        private bool courseExists(string cid)
        {
            return db.courses.Count(e => e.courseID == cid) > 0;

        }
    }
}
