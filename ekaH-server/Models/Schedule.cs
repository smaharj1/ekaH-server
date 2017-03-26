using ekaH_server.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    public class FullScheduleInfo
    {
        public Schedule Schedule { get; set; }
        public FacultyInfo Faculty { get; set; }
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

        public List<Appointment> divideToAppointments()
        {
            List<Appointment> allAppointments = new List<Appointment>();

            for (DateTime dt = StartDTime; dt < EndDTime; dt = dt.AddMinutes(30))
            {
                Appointment app = new Appointment();
                app.ScheduleID = ScheduleID;
                app.AttendeeID = "";
                app.Confirmed = false;
                app.StartTime = dt;
                app.EndTime = app.StartTime.AddMinutes(30);

                allAppointments.Add(app);
            }

            return allAppointments;
        }
    }

    
}