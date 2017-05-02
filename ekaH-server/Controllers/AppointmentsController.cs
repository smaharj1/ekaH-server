using ekaH_server.App_DBHandler;
using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    /// <summary>
    /// This class handles all the functionalities of modifying/creating appointments.
    /// </summary>
    public class appointmentsController : ApiController
    {
        /// <summary>
        /// It holds the entity for the database connection.
        /// </summary>
        private ekahEntities11 m_db = new ekahEntities11();


        /// <summary>
        /// GET: ekah/appointments/{action}/{id}
        /// action: facultySchedule, id = Optional
        /// This function returns the weekly schedule fo a professor mentioned in id.
        /// </summary>
        /// <param name="id">It holds the email id of the professor.</param>
        /// <returns>Returns the weekly schedule of a professor mentioned in id.</returns>
        [HttpGet]
        [ActionName("facultySchedule")]
        public IHttpActionResult GetSchedule(string id)
        {
            /// Initializes the dates to find the schedule for.
            Schedule schedule = new Schedule();
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(7);

            /// Gets all the office hours by the professor for a week.
            List<officehour> officehours = m_db.officehours.Where(oh => oh.startDTime <= currentDate
                && oh.endDTime <= futureDate && oh.professorID == id).ToList();

            schedule.StartDate = currentDate;
            schedule.EndDate = futureDate;
            schedule.ProfessorID = id;
            List<DayInfo> days = new List<DayInfo>();

            /// Parses the office hours into the days.
            foreach( officehour oneEntry in officehours)
            {
                DayInfo tempDay = new DayInfo();
                DateTime startDTime = oneEntry.startDTime;
                DateTime endDTime = oneEntry.endDTime;

                tempDay.Day = startDTime.DayOfWeek;
                tempDay.startTime = startDTime.TimeOfDay;
                tempDay.endTime = endDTime.TimeOfDay;

                days.Add(tempDay);
            }
            schedule.Days = days.ToArray();

            return Ok(schedule);
            

        }


        /// <summary>
        /// GET: ekah/appointments/{action}/{id}
        /// action: appointment, id = Optional
        /// It returns all the available appointment times information of a professor mentioned in id.
        /// </summary>
        /// <param name="id">It holds the professor's email id.</param>
        /// <returns>Returns all the available appointment times information of a professor mentioned in id.</returns>
        [HttpGet]
        [ActionName("schedules")]
        public IHttpActionResult GetSchedules(string id)
        {
            List<appointment> appointments = GetTwoWeekSchedule(id);

            return Ok(appointments);
        }

        /// <summary>
        /// This function returns all the available appointments for the next two weeks.
        /// </summary>
        /// <param name="a_email">It holds the email of the professor.</param>
        /// <returns>Returns the available appointments for next two weeks.</returns>
        public List<appointment> GetTwoWeekSchedule(string a_email)
        {
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(14);

            /// Gets all the office hours for 2 weeks.
            List<officehour> officeHours = m_db.officehours.Where(oh => oh.startDTime >= currentDate && 
                oh.startDTime <= futureDate && oh.professorID == a_email).ToList();

            List<appointment> generatedApps = new List<appointment>();
            List<int> scheduleIDs = new List<int>();

            /// Goes through the office hours and parses it into appointments of 30 minute intervals.
            foreach (officehour singleEntry in officeHours)
            {
                scheduleIDs.Add(singleEntry.id);
                SingleSchedule sch = new SingleSchedule();
                sch.ProfessorID = a_email;
                sch.ScheduleID = singleEntry.id;
                sch.StartDTime = singleEntry.startDTime;
                sch.EndDTime = singleEntry.endDTime;

                generatedApps.AddRange(sch.divideToAppointments());
            }

            List<appointment> reservedApps = m_db.appointments.Where(app => scheduleIDs.Contains(app.scheduleID) ).ToList();
            
            List<appointment> result = new List<appointment>();

            /// Separates the reserved appointment times so that it is not sent.
            foreach (appointment apps in generatedApps)
            {
                bool contains = false;
                foreach (appointment reserved in reservedApps)
                {
                    if (apps.scheduleID == reserved.scheduleID && apps.startTime == reserved.startTime)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    result.Add(apps);
                }
            }

            return result;
        }


        /// <summary>
        /// POST: ekah/appointments/{action}/{id}
        /// action: schedule
        /// This function posts the schedule of a given professor for the given times
        /// </summary>
        /// <param name="a_schedule">It holds the schedule to be posted.</param>
        /// <returns>Returns the result of posting.</returns>
        [HttpPost]
        [ActionName("schedule")]
        public IHttpActionResult PostSchedule([FromBody] Schedule a_schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return PostScheduleToDB(a_schedule);
        }

        /// <summary>
        /// This function deletes the schedule from the database.
        /// </summary>
        /// <param name="id">It holds the schedule ID.</param>
        /// <returns>Returns the result of deletion.</returns>
        [HttpDelete]
        [ActionName("schedule")]
        public IHttpActionResult DeleteSchedule(int id)
        {
            officehour officeHour = m_db.officehours.Find(id);

            if (officeHour == null)
            {
                return NotFound();
            }

            /// Removes it from the database.
            m_db.officehours.Remove(officeHour);
            m_db.SaveChanges();

            return Ok();
        }


        // -------------------------------------------------------------------
        // --------------- APPOINTMENT PART-----------------------------------
        // -------------------------------------------------------------------

        /// <summary>
        /// POST: ekah/appointments/{action}/{id}
        /// Action: app
        /// This function adds an appointment to the database.
        /// </summary>
        /// <param name="a_appointment">It holds the appointment to be added.</param>
        /// <returns>Returns the result of addition.</returns>
        [HttpPost]
        [ActionName("app")]
        public IHttpActionResult AddAppointment([FromBody]appointment a_appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /// Checks if appointment already exists.
            if (AppointmentExists(a_appointment))
            {
                return Conflict();
            }

            m_db.appointments.Add(a_appointment);

            try
            {
                m_db.SaveChanges();
            }
            catch(Exception)
            {
                return InternalServerError();
            }

            return Ok();

        }

        /// <summary>
        /// POST: ekah/appointments/{action}/{id}
        /// Action: app
        /// This function modifies the current appointment.
        /// </summary>
        /// <param name="id">It holds the id of the appointment.</param>
        /// <param name="a_appointment">It holds the appointment.</param>
        /// <returns>Returns the result of modification.</returns>
        [HttpPut]
        [ActionName("app")]
        public IHttpActionResult ModifyAppointment(int id, [FromBody]appointment a_appointment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != a_appointment.id)
            {
                return BadRequest();
            }

            /// Checks if appointment exists. For modification, appointment needs to exist.
            if (!AppointmentExists(id))
            {
                return NotFound();
            }

            m_db.Entry(a_appointment).State = EntityState.Modified;

            try
            {
                m_db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// GET: ekah/appointments/{action}/{id}
        /// action: schedule, id = Optional
        /// This function returns the full schedule (Schedule and professor info)
        /// </summary>
        /// <param name="id">It holds the id.</param>
        /// <returns>Returns the full schedule information.</returns>
        [HttpGet]
        [ActionName("appFull")]
        public IHttpActionResult GetFullInfo(int id)
        {
            appointment app = m_db.appointments.Find(id);

            if (app == null)
            {
                return NotFound();
            }

            /// Gets the student's and professor's information.
            student_info student = app.student_info;
            officehour oHour = app.officehour;

            professor_info professor = m_db.professor_info.Find(oHour.professor_info.email);

            FullAppointmentInfo fullInfo = new FullAppointmentInfo();
            fullInfo.Appointment= app;
            fullInfo.Student = student;
            fullInfo.Faculty = professor;

            if (app == null)
            {
                return NotFound();
            }

            return Ok(fullInfo);

        }

        /// <summary>
        /// GET: ekah/appointments/{action}/
        /// Action: app. This gets all the appointments in the database. For admin purposes.
        /// </summary>
        /// <returns>Returns all the appointments.</returns>
        [HttpGet]
        [ActionName("app")]
        public IHttpActionResult GetAllAppointments()
        {
            List<appointment> app = m_db.appointments.ToList();

            return Ok(app);
            
        }

        /// <summary>
        /// DELETE: ekah/appointments/{action}/{id}
        /// Action: app. Deletes the appointment with the given ID
        /// </summary>
        /// <param name="id">It holds the appointment id.</param>
        /// <returns>Returns the result after deletion.</returns>
        [HttpDelete]
        [ActionName("app")]
        public IHttpActionResult DeleteAppointment(int id)
        {
            appointment app = m_db.appointments.Find(id);

            m_db.appointments.Remove(app);
            m_db.SaveChanges();

            return Ok();

        }

        // -------------------------------------------------------------------
        // --------------- STUDENT/FACULTY APPOINTMENT PART---------------------------
        // -------------------------------------------------------------------

        /// <summary>
        /// GET: ekah/appointments/{action}/{id}
        /// Action: students. Gets all the appointments currently held by a student.
        /// id represents the email address of a student
        /// </summary>
        /// <param name="id"> It holds the email address of the student.</param>
        /// <returns>Returns all the existing student appointments.</returns>
        [HttpGet]
        [ActionName("students")]
        public IHttpActionResult GetStudentAppointments(string id)
        {
            List<appointment> apps = m_db.appointments.Where(appointment => appointment.attendeeID == id).ToList();

            return Ok(apps);
        }

        /// <summary>
        /// GET: ekah/appointments/{action}/{id}
        /// Action: app. Default GET accepts id as EMAIL of professor
        /// </summary>
        /// <param name="id">It holds the email address of professor.</param>
        /// <returns>Returns all the appointments of the professor.</returns>
        [HttpGet]
        [ActionName("faculties")]
        public IHttpActionResult GetAppointments(string id)
        {
            /// Gets all the appointments for next 2 weeks.
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(14);
            List<officehour> officeHours = m_db.officehours.Where(oh => oh.startDTime >= currentDate && oh.startDTime <= futureDate && oh.professorID == id).ToList();
            List<int> officeHourIds = new List<int>();
            foreach(officehour temp in officeHours)
            {
                officeHourIds.Add(temp.id);
            }

            List<appointment> appointments = m_db.appointments.Where(app => officeHourIds.Contains(app.scheduleID)).ToList();

            return Ok(appointments);
            
        }

        // Posts the schedule to the database.
        /// <summary>
        /// This function posts the schedule to the database.
        /// </summary>
        /// <param name="a_schedule">It holds the schedule to be posted.</param>
        /// <returns>Returns the result after posting.</returns>
        public IHttpActionResult PostScheduleToDB([FromBody] Schedule a_schedule)
        {
            /// Adds the schedule in the day by day format that represents a single time frame for an entry.
            for (DateTime tempDate = a_schedule.StartDate; tempDate < a_schedule.EndDate; tempDate = tempDate.AddDays(1))
            {
                foreach (DayInfo singleDay in a_schedule.Days)
                {
                    if (tempDate.DayOfWeek == singleDay.Day)
                    {
                        DateTime startDTime = tempDate;
                        startDTime = startDTime.Date + singleDay.startTime;

                        DateTime endDTime = tempDate;
                        endDTime = endDTime.Date + singleDay.endTime;

                        officehour single = new officehour();
                        single.professorID = a_schedule.ProfessorID;
                        single.startDTime = startDTime;
                        single.endDTime = endDTime;

                        m_db.officehours.Add(single);
                    }
                }
            }
            try
            {
                m_db.SaveChanges();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// This function checks if the appointment exists in the database.
        /// </summary>
        /// <param name="a_app">It holds if the appointment exists.</param>
        /// <returns>Returns true if appointment exists.</returns>
        private bool AppointmentExists(appointment a_app)
        {
            /// Checks if the existing appointment is already confirmed. 
            /// If it is confirmed, then only appointment is said to exist.
            bool isConfirmed = Convert.ToBoolean(a_app.confirmed);
            return m_db.appointments.Count(a => a.scheduleID == a_app.scheduleID &&
            a.startTime == a_app.startTime && isConfirmed == true) != 0;
        }

        /// <summary>
        /// This function checks if the appointment exists given the appointment id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool AppointmentExists(int id)
        {
            return m_db.appointments.Count(app => app.id == id) > 0;
        }

    }
}
