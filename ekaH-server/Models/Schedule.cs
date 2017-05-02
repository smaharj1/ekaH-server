using ekaH_server.App_DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    /// <summary>
    /// This class holds the full information of an appointment.
    /// </summary>
    public class FullAppointmentInfo
    {
        /// <summary>
        /// It holds the appointment.
        /// </summary>
        public appointment Appointment { get; set; }

        /// <summary>
        /// It holds the faculty information.
        /// </summary>
        public professor_info Faculty { get; set; }

        /// <summary>
        /// It holds the student information.
        /// </summary>
        public student_info Student { get; set; }
    }

    /// <summary>
    /// This class holds the schedule of a professor.
    /// </summary>
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public string ProfessorID { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public DayInfo[] Days { get; set; }
    }
    
    /// <summary>
    /// This class holds the single schedule of the professor.
    /// </summary>
    public class SingleSchedule
    {
        public int ScheduleID { get; set; }
        public string ProfessorID { get; set; }
        public DateTime StartDTime { get; set; }
        public DateTime EndDTime { get; set; }

        /// <summary>
        /// This function divides a schedule to appointments in 30-minute time interval.
        /// </summary>
        /// <returns>Returns all the available appointments</returns>
        public List<appointment> divideToAppointments()
        {
            List<appointment> allAppointments = new List<appointment>();

            /// Gets all the appointments from the given schedule.
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