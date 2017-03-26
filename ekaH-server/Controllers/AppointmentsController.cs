﻿using ekaH_server.App_DBHandler;
using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    public class appointmentsController : ApiController
    {
        
        // GET: ekah/appointments/{action}/{id}
        // action: facultySchedule, id = Optional
        // Returns the weekly schedule of a professor mentioned in id.
        [HttpGet]
        [ActionName("facultySchedule")]
        public IHttpActionResult GetSchedule(string id)
        {
            try
            {
                // Gets the schedule of the current week in Schedule object.
                Schedule schedule = AppointmentDBHandler.getScheduleByProfessorID(id);
                return Ok(schedule);
            }
            catch(Exception)
            {
                return InternalServerError();
            }

        }

        // GET: ekah/appointments/{action}/{id}
        // action: schedule, id = Optional
        // Returns the full schedule (Schedule and professor info)
        [HttpGet]
        [ActionName("schedule")]
        public IHttpActionResult GetFullInfo(int id)
        {
            try
            {
                // Gets the schedule of the current week in Schedule object.
                FullScheduleInfo fullSchedule = AppointmentDBHandler.getFullScheduleInfo(id);

                if (fullSchedule != null) return Ok(fullSchedule);
                else return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        // GET: ekah/appointments/{action}/{id}
        // action: appointment, id = Optional
        // Returns all the available appointment times information of a professor mentioned in id.
        [HttpGet]
        [ActionName("schedules")]
        public IHttpActionResult GetSchedules(string id)
        {
            List<Appointment> allSchedule;
            try
            {
                // Gets the schedule of the current week in Schedule object.
                allSchedule = AppointmentDBHandler.getTwoWeekSchedule(id);
                
                return Ok(allSchedule);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        // POST: ekah/appointments/{action}/{id}
        // action: schedule
        // Posts the schedule of a given professor for the given times
        [HttpPost]
        [ActionName("schedule")]
        public IHttpActionResult PostSchedule([FromBody] Schedule schedule)
        {
            bool status = true;
            try
            {
                status = AppointmentDBHandler.postScheduleToDB(schedule);
            }
            catch(Exception)
            {
                return InternalServerError();
            }

            if (status)
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
           
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
            bool status = AppointmentDBHandler.deleteScheduleFromDB(id);

            if (status)
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }


        // -------------------------------------------------------------------
        // --------------- APPOINTMENT PART-----------------------------------
        // -------------------------------------------------------------------

        // POST: ekah/appointments/{action}/{id}
        // Action: app
        [HttpPost]
        [ActionName("app")]
        public IHttpActionResult AddAppointment([FromBody]Appointment appointment)
        {
            try
            {
                bool status = AppointmentDBHandler.postAppointment(appointment);
                if (status) return Ok();
                else return Conflict();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        

        // GET: ekah/appointments/{action}/
        // Action: app. This gets all the appointments in the database. For admin purposes.
        [HttpGet]
        [ActionName("app")]
        public IHttpActionResult GetAllAppointments()
        {
            List<Appointment> allAppointments = new List<Appointment>();

            try
            {
                allAppointments = AppointmentDBHandler.getAllAppointments();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(allAppointments);
        }

        // DELETE: ekah/appointments/{action}/{id}
        // Action: app. Deletes the appointment with the given ID
        [HttpDelete]
        [ActionName("app")]
        public IHttpActionResult DeleteAppointment(int id)
        {
            try
            {
                AppointmentDBHandler.deleteAppointment(id);
                return Ok();
            }
            catch(Exception)
            {
                return InternalServerError();
            }
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
            }
        }

        // GET: ekah/appointments/{action}/{id}
        // Action: app. Default GET accepts id as EMAIL of professor
        [HttpGet]
        [ActionName("faculties")]
        public IHttpActionResult GetAppointments(string id)
        {
            List<Appointment> allAppointments = new List<Appointment>();

            try
            {
                allAppointments = AppointmentDBHandler.getAppointmentsByProfessorID(id, 2);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(allAppointments);
        }

    }
}
