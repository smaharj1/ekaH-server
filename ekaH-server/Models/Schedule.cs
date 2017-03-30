using ekaH_server.App_DBHandler;
using ekaH_server.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    public class FullAppointmentInfo
    {
        public appointment Appointment { get; set; }
        public professor_info Faculty { get; set; }
        public student_info Student { get; set; }
    }
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public string ProfessorID { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public DayInfo[] Days { get; set; }
    }

    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int ScheduleID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AttendeeID { get; set; }
        public bool Confirmed { get; set; }
    }

    public class SingleSchedule
    {
        public int ScheduleID { get; set; }
        public string ProfessorID { get; set; }
        public DateTime StartDTime { get; set; }
        public DateTime EndDTime { get; set; }

        public List<appointment> divideToAppointments()
        {
            List<appointment> allAppointments = new List<appointment>();

            for (DateTime dt = StartDTime; dt < EndDTime; dt = dt.AddMinutes(30))
            {
                appointment app = new appointment();
                app.scheduleID = ScheduleID;
                app.attendeeID = "";
                app.confirmed = 0;
                app.startTime = dt;
                app.endTime = app.startTime.AddMinutes(30);

                allAppointments.Add(app);
            }

            return allAppointments;
        }
    }

    
}