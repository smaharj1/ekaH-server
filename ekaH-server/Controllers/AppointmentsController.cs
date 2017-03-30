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
    public class appointmentsController : ApiController
    {
        private ekahEntities11 db = new ekahEntities11();


        // GET: ekah/appointments/{action}/{id}
        // action: facultySchedule, id = Optional
        // Returns the weekly schedule of a professor mentioned in id.
        [HttpGet]
        [ActionName("facultySchedule")]
        public IHttpActionResult GetSchedule(string id)
        {
            Schedule schedule = new Schedule();
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(7);

            List<officehour> officehours = db.officehours.Where(oh => oh.startDTime <= currentDate
                && oh.endDTime <= futureDate && oh.professorID == id).ToList();

            schedule.StartDate = currentDate;
            schedule.EndDate = futureDate;
            schedule.ProfessorID = id;
            List<DayInfo> days = new List<DayInfo>();

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

        

        // GET: ekah/appointments/{action}/{id}
        // action: appointment, id = Optional
        // Returns all the available appointment times information of a professor mentioned in id.
        [HttpGet]
        [ActionName("schedules")]
        public IHttpActionResult GetSchedules(string id)
        {
            List<appointment> appointments = getTwoWeekSchedule(id);

            return Ok(appointments);


        }


        // Gets all the available two weeks appointments where the user can book.
        private List<appointment> getTwoWeekSchedule(string email)
        {
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(14);

            List<officehour> officeHours = db.officehours.Where(oh => oh.startDTime >= currentDate && 
                oh.startDTime <= futureDate && oh.professorID == email).ToList();

            List<appointment> generatedApps = new List<appointment>();
            List<int> scheduleIDs = new List<int>();

            foreach (officehour singleEntry in officeHours)
            {
                scheduleIDs.Add(singleEntry.id);
                SingleSchedule sch = new SingleSchedule();
                sch.ProfessorID = email;
                sch.ScheduleID = singleEntry.id;
                sch.StartDTime = singleEntry.startDTime;
                sch.EndDTime = singleEntry.endDTime;

                generatedApps.AddRange(sch.divideToAppointments());
            }

            List<appointment> reservedApps = db.appointments.Where(app => scheduleIDs.Contains(app.scheduleID) ).ToList();
            
            List<appointment> result = new List<appointment>();

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
        

        // POST: ekah/appointments/{action}/{id}
        // action: schedule
        // Posts the schedule of a given professor for the given times
        [HttpPost]
        [ActionName("schedule")]
        public IHttpActionResult PostSchedule([FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return postScheduleToDB(schedule);
            
        }


        // PUT: api/Appointment/5
        // TODO: Do it in later times.
        [HttpPut]
        [ActionName("schedule")]
        public void ModifySchedule([FromBody]Schedule schedule)
        {
        }

        // DELETE: ekah/appointments/{action}/{id}
        [HttpDelete]
        [ActionName("schedule")]
        public IHttpActionResult DeleteSchedule(int id)
        {
            officehour officeHour = db.officehours.Find(id);

            if (officeHour == null)
            {
                return NotFound();
            }

            db.officehours.Remove(officeHour);
            db.SaveChanges();

            return Ok();
            
        }


        // -------------------------------------------------------------------
        // --------------- APPOINTMENT PART-----------------------------------
        // -------------------------------------------------------------------

        // POST: ekah/appointments/{action}/{id}
        // Action: app
        [HttpPost]
        [ActionName("app")]
        public IHttpActionResult AddAppointment([FromBody]appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (appointmentExists(appointment))
            {
                return Conflict();
            }

            db.appointments.Add(appointment);

            try
            {
                db.SaveChanges();
            }
            catch(Exception)
            {
                return InternalServerError();
            }

            return Ok();

        }

        

        // POST: ekah/appointments/{action}/{id}
        // Action: app
        [HttpPut]
        [ActionName("app")]
        public IHttpActionResult ModifyAppointment(int id, [FromBody]appointment appointment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.id)
            {
                return BadRequest();
            }

            if (!appointmentExists(id))
            {
                return NotFound();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);

           
        }

        // GET: ekah/appointments/{action}/{id}
        // action: schedule, id = Optional
        // Returns the full schedule (Schedule and professor info)
        [HttpGet]
        [ActionName("appFull")]
        public IHttpActionResult GetFullInfo(int id)
        {
            appointment app = db.appointments.Find(id);

            if (app == null)
            {
                return NotFound();
            }
            student_info student = app.student_info;
            officehour oHour = app.officehour;

            professor_info professor = db.professor_info.Find(oHour.professor_info.email);

            FullAppointmentInfo fullInfo = new FullAppointmentInfo();
            fullInfo.Appointment= app;
            fullInfo.Student = student;
            fullInfo.Faculty = professor;

            if (app == null)
            {
                return NotFound();
            }

            return Ok(fullInfo);

            /*
            try
            {
                // Gets the schedule of the current week in Schedule object.
                FullAppointmentInfo fullSchedule = AppointmentDBHandler.getFullAppointmentInfo(id);

                if (fullSchedule != null) return Ok(fullSchedule);
                else return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }*/
        }

        // GET: ekah/appointments/{action}/
        // Action: app. This gets all the appointments in the database. For admin purposes.
        [HttpGet]
        [ActionName("app")]
        public IHttpActionResult GetAllAppointments()
        {
            List<appointment> app = db.appointments.ToList();

            return Ok(app);

            /*
            List<Appointment> allAppointments = new List<Appointment>();

            try
            {
                allAppointments = AppointmentDBHandler.getAllAppointments();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(allAppointments);*/
        }

        // DELETE: ekah/appointments/{action}/{id}
        // Action: app. Deletes the appointment with the given ID
        [HttpDelete]
        [ActionName("app")]
        public IHttpActionResult DeleteAppointment(int id)
        {
            appointment app = db.appointments.Find(id);

            db.appointments.Remove(app);
            db.SaveChanges();

            return Ok();

            /*
            try
            {
                AppointmentDBHandler.deleteAppointment(id);
                return Ok();
            }
            catch(Exception)
            {
                return InternalServerError();
            }*/
        }

        // -------------------------------------------------------------------
        // --------------- STUDENT/FACULTY APPOINTMENT PART---------------------------
        // -------------------------------------------------------------------

        // GET: ekah/appointments/{action}/{id}
        // Action: students. Gets all the appointments currently held by a student.
        // id represents the email address of a student
        [HttpGet]
        [ActionName("students")]
        public IHttpActionResult GetStudentAppointments(string id)
        {
            List<appointment> apps = db.appointments.Where(appointment => appointment.attendeeID == id).ToList();

            return Ok(apps);

            /*
            List<Appointment> appointments;

            try
            {
                appointments = AppointmentDBHandler.getAppointmentsByStudent(id);
                return Ok(appointments);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return InternalServerError();
            }*/
        }

        // GET: ekah/appointments/{action}/{id}
        // Action: app. Default GET accepts id as EMAIL of professor
        [HttpGet]
        [ActionName("faculties")]
        public IHttpActionResult GetAppointments(string id)
        {
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(14);
            List<officehour> officeHours = db.officehours.Where(oh => oh.startDTime >= currentDate && oh.startDTime <= futureDate && oh.professorID == id).ToList();
            List<int> officeHourIds = new List<int>();
            foreach(officehour temp in officeHours)
            {
                officeHourIds.Add(temp.id);
            }

            List<appointment> appointments = db.appointments.Where(app => officeHourIds.Contains(app.scheduleID)).ToList();

            return Ok(appointments);

            /*
            try
            {
                allAppointments = AppointmentDBHandler.getAppointmentsByProfessorID(id, 2);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(allAppointments);*/
        }

        // Posts the schedule to the database.
        public IHttpActionResult postScheduleToDB(Schedule schedule)
        {
            string requestQuery = "";

            for (DateTime tempDate = schedule.StartDate; tempDate < schedule.EndDate; tempDate = tempDate.AddDays(1))
            {
                foreach (DayInfo singleDay in schedule.Days)
                {
                    if (tempDate.DayOfWeek == singleDay.Day)
                    {
                        DateTime startDTime = tempDate;
                        startDTime = startDTime.Date + singleDay.startTime;

                        DateTime endDTime = tempDate;
                        endDTime = endDTime.Date + singleDay.endTime;

                        officehour single = new officehour();
                        single.professorID = schedule.ProfessorID;
                        single.startDTime = startDTime;
                        single.endDTime = endDTime;

                        db.officehours.Add(single);
                    }
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();


        }

        private bool appointmentExists(appointment app)
        {
            bool isConfirmed = Convert.ToBoolean(app.confirmed);
            return db.appointments.Count(a => a.scheduleID == app.scheduleID &&
            a.startTime == app.startTime && isConfirmed == true) != 0;
        }

        private bool appointmentExists(int id)
        {
            return db.appointments.Count(app => app.id == id) > 0;
        }

    }
}
