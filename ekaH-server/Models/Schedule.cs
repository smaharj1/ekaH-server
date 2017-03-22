using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    public class Schedule
    {
        public string ProfessorID { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public DayInfo[] Days { get; set; }
    }

    public class SingleSchedule
    {
        public int ScheduleID { get; set; }
        public string ProfessorID { get; set; }
        public DateTime StartDTime { get; set; }
        public DateTime EndDTime { get; set; }
    }

    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int ScheduleID { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string AttendeeID { get; set; }
        public bool Confirmed { get; set; }
    }
}