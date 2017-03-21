using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public string ProfessorID { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public DayInfo[] Days { get; set; }
    }
}