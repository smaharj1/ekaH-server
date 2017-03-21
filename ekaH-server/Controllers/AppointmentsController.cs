using ekaH_server.App_DBHandler;
using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    public class appointmentsController : ApiController
    {
        // GET: ekah/appointments/{action}/{id}
        // Returns all the appointments in the database by all the people.
        [HttpGet]
        [ActionName("appointment")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: ekah/appointments/{action}/{id}
        // action: appointment, id = Optional
        // Returns the appointment information of a particular appointment ID. 
        [HttpGet]
        [ActionName("appointment")]
        public string GetSchedule(int id)
        {
            return "value";
        }

        // POST: ekah/appointments/{action}/{id}
        // action: appointment
        // Posts an appointment to the given professor
        public void Post([FromBody]string value)
        {
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
                status = ScheduleDBHandler.postScheduleToDB(schedule);
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
        public IHttpActionResult DeleteSchedule([FromBody] Schedule schedule)
        {
            bool status = true;
            try
            {
                status = ScheduleDBHandler.deleteScheduleFromDB(schedule);
            }
            catch (Exception)
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
    }
}
