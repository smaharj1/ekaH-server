using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using ekaH_server.Models.UserModels;

namespace ekaH_server.App_DBHandler
{
    public class AppointmentDBHandler
    {
        // Returns the SQL safe date string.
        private static string getDateString(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private static string getTimeString(DateTime dtime)
        {
            return dtime.ToString();
        }

        private static Appointment generateAppointment(MySqlDataReader reader)
        {
            Appointment tempApp = new Appointment();
            tempApp.AppointmentID = (int)reader.GetValue(0);
            tempApp.ScheduleID = (int)reader.GetValue(1);
            tempApp.StartTime = reader.GetDateTime(2);
            tempApp.EndTime = reader.GetDateTime(3);
            tempApp.AttendeeID = reader.IsDBNull(4) ? "" : reader.GetString(4);
            tempApp.Confirmed = reader.GetBoolean(5);

            return tempApp;
        }

        // Posts the schedule to the database.
        public static bool postScheduleToDB(Schedule schedule)
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

                        // Inserts the info to the table only if it does not exist. If it exists, it simply updates it.
                        requestQuery += "insert into officehours(professorID, startDtime, endDTime) values ('" + 
                            schedule.ProfessorID +"', '" + getDateString(startDTime)+"', '"+
                            getDateString(endDTime) +"') on duplicate key update professorID='"+
                            schedule.ProfessorID+"', startDTime='"+ getDateString(startDTime)+"', endDTime='"+ getDateString(endDTime) +"';";
                    }
                }
            }

            DBConnection db = DBConnection.getInstance();

            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                cmd.ExecuteNonQuery();
            }
            catch(MySqlException)
            {
                return false;
            }

            return true;
        }

        // Deletes the certain schedule with the id given.
        public static bool deleteScheduleFromDB(int id)
        {
            DBConnection db = DBConnection.getInstance();
            string requestQuery = "delete * from officehours where id=" + id + ";";
            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                return false;
            }

            return true;
        }

        // Gets the schedule of a professor in a week.
        public static Schedule getScheduleByProfessorID(string professorID)
        {
            DBConnection db = DBConnection.getInstance();
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(7);

            string requestQuery = "select * from officehours where startDTime >= '" + getDateString(currentDate) +
                "' and startDTime <= '" + getDateString(futureDate) + "' and professorID='"+professorID+"';";

            MySqlDataReader reader = null;

            Schedule schedule = new Schedule();
            schedule.StartDate = currentDate;
            schedule.EndDate = futureDate;
            schedule.ProfessorID = professorID;

            List<DayInfo> days = new List<DayInfo>();
            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DayInfo tempDay = new DayInfo();
                    DateTime startDTime = reader.GetDateTime(2);
                    DateTime endDTime = reader.GetDateTime(3);

                    tempDay.Day = startDTime.DayOfWeek;
                    tempDay.startTime = startDTime.TimeOfDay;
                    tempDay.endTime = endDTime.TimeOfDay;

                    days.Add(tempDay);
                }
            }
            catch(MySqlException)
            {
                throw new Exception();
            }

            schedule.Days = days.ToArray();
            reader.Dispose();
            return schedule;
        }

        // Gets all the available two weeks appointments where the user can book.
        public static List<Appointment> getTwoWeekSchedule(string email)
        {
            DBConnection db = DBConnection.getInstance();
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(14);

            string requestQuery = "select * from officehours where startDTime >= '" + getDateString(currentDate) +
                "' and startDTime <= '" + getDateString(futureDate) + "' and professorID='" + email + "';";

            List<SingleSchedule> schedules = new List<SingleSchedule>();
            List<Appointment> generatedApps = new List<Appointment>();

            MySqlDataReader reader = null;

            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SingleSchedule sch = new SingleSchedule();
                    sch.ProfessorID = email;
                    sch.ScheduleID = (int)reader.GetValue(0);
                    sch.StartDTime = reader.GetDateTime(2);
                    sch.EndDTime = reader.GetDateTime(3);

                    schedules.Add(sch);
                    generatedApps.AddRange(sch.divideToAppointments());
                }

            }
            catch(MySqlException ex)
            {
                throw ex;
            }

            reader.Dispose();

            List<Appointment> reservedApps;
            try
            {
                 reservedApps = getAppointmentsByProfessorID(email, 2);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            List<Appointment> result = new List<Appointment>();

            foreach (Appointment apps in generatedApps)
            {
                bool contains = false;
                foreach(Appointment reserved in reservedApps)
                {
                    if (apps.ScheduleID == reserved.ScheduleID && apps.StartTime == reserved.StartTime)
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


        public static bool checkIfAppointmentExists(Appointment appointment)
        {
            DBConnection db = DBConnection.getInstance();

            string requestQuery = "select * from appointments where scheduleID = " + appointment.ScheduleID + " and startTime = '" +
                getDateString(appointment.StartTime) + "' and endTime = '" + getDateString(appointment.EndTime) + "';";

            MySqlDataReader reader = null;
            bool result = true;
            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                if (reader.Read()) result = true;
                else result =  false;
            }
            catch (MySqlException ee)
            {
                Debug.WriteLine(ee.Message);
                throw new Exception();
            }

            reader.Dispose();
            return result;
        }

        // Sets an appointment. It is assumed that the client system already verifies that the professor's
        // schedule is configured and ID is captured.
        public static bool postAppointment(Appointment appointment)
        {
            DBConnection db = DBConnection.getInstance();

            if (checkIfAppointmentExists(appointment))
            {
                return false;
            }

            string requestQuery = "insert into appointments(scheduleID, startTime, endTime, attendeeID)"+
                " values (@scheduleID, @startTime, @endTime, @attendeeID);";

            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                cmd.Prepare();

                cmd.Parameters.AddWithValue("scheduleID", appointment.ScheduleID);
                cmd.Parameters.AddWithValue("startTime", getDateString(appointment.StartTime));
                cmd.Parameters.AddWithValue("endTime", getDateString(appointment.EndTime));
                cmd.Parameters.AddWithValue("attendeeID", appointment.AttendeeID);

                cmd.ExecuteNonQuery();
            }
            catch(MySqlException ex)
            {
                throw ex;
            }

            return true;
        }

        // Modifies the currently existing appointment
        public static bool putAppointment(Appointment appointment)
        {
            DBConnection db = DBConnection.getInstance();

            if (!checkIfAppointmentExists(appointment))
            {
                return false;
            }

            string requestQuery = "update appointments set scheduleID=@schID, startTime=@start"+
                ", endTime=@end, attendeeID=@student, confirmed=@confirm where id=@appID;";

            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                cmd.Prepare();

                cmd.Parameters.AddWithValue("schID", appointment.ScheduleID);
                cmd.Parameters.AddWithValue("start", getDateString(appointment.StartTime));
                cmd.Parameters.AddWithValue("end", getDateString(appointment.EndTime));
                cmd.Parameters.AddWithValue("student", appointment.AttendeeID);
                cmd.Parameters.AddWithValue("confirm", appointment.Confirmed ? 1 : 0);
                cmd.Parameters.AddWithValue("appID", appointment.AppointmentID);

                cmd.ExecuteNonQuery();
            }
            catch(MySqlException ex)
            {
                throw ex;
            }
            

            return true;
        }

        public static List<Appointment> getAppointmentsByProfessorID(string email, int weeks)
        {
            DBConnection db = DBConnection.getInstance();
            DateTime currentDate = DateTime.Now;
            DateTime futureDate = currentDate.AddDays(weeks * 7);

            string requestQuery = "select * from appointments where scheduleID in (select id from officehours where startDTime >= '" +
                getDateString(currentDate) + "' and startDTime <= '" + getDateString(futureDate) + "' and professorID='" + email + "');";

            try
            {
                List<Appointment> list = getAppointmentsByQuery(requestQuery);
                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public static List<Appointment> getAllAppointments()
        {

            string requestQuery = "select * from appointments;";

            try
            {
                List<Appointment> list = getAppointmentsByQuery(requestQuery);
                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static List<Appointment> getAppointmentsByStudent(string id)
        {
            DBConnection db = DBConnection.getInstance();

            MySqlDataReader reader = null;

            List<Appointment> list = new List<Appointment>();

            string query = "select * from appointments where attendeeID = @emailID;";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Prepare();
                cmd.Parameters.AddWithValue("emailID", id);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Appointment app = generateAppointment(reader);
                    list.Add(app);
                }
                
            }
            catch(MySqlException ex)
            {
                if (reader != null) reader.Dispose();
                throw ex;
            }

            reader.Dispose();

            return list;
        }

        // This gets the list of appointments by the given query.
        private static List<Appointment> getAppointmentsByQuery(string requestQuery)
        {
            DBConnection db = DBConnection.getInstance();

            MySqlDataReader reader = null;

            List<Appointment> list = new List<Appointment>();
            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Appointment tempApp = UserWorker.extractAppointment(reader);
                    list.Add(tempApp);
                }
            }
            catch (MySqlException ee)
            {
                string exc = ee.Message;
                throw new Exception();
            }

            reader.Dispose();

            return list;
        }

        public static bool deleteAppointment(int id)
        {
            DBConnection db = DBConnection.getInstance();

            string query = "delete from appointments where id=" + id + ";";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.ExecuteNonQuery();
            }
            catch(MySqlException)
            {
                throw new Exception();
            }

            return true;
        }

        // id holds the appointment ID
        // Returns the full information of appointment including professor and student info.
        public static FullAppointmentInfo getFullAppointmentInfo(int id)
        {
            DBConnection db = DBConnection.getInstance();

            string query = "select * from professor_info where " +
                "professor_info.email = (select professorID from officehours where officehours.id = "+
                "(select scheduleID from appointments where id=@appointmentID));";

            MySqlDataReader reader = null;
            FullAppointmentInfo fullInfo = new FullAppointmentInfo();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Prepare();

                cmd.Parameters.AddWithValue("appointmentID", id);

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    FacultyInfo faculty = UserWorker.extractFaculty(reader);
                    fullInfo.Faculty = faculty;
                }
            }
            catch(MySqlException ex)
            {
                throw ex;
            }

            reader.Dispose();

            // Gets the appointment object
            query = "select * from appointments where id = @appointmentID;";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, db.getConnection());
                cmd.Prepare();

                cmd.Parameters.AddWithValue("appointmentID", id);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Appointment app = UserWorker.extractAppointment(reader);
                    fullInfo.Appointment = app;
                }
            }
            catch(MySqlException ex)
            {
                throw ex;
            }

            reader.Dispose();

            // Populates the information of the student
            if (!String.IsNullOrEmpty(fullInfo.Appointment.AttendeeID))
            {
                StudentInfo student = FacultyDBHandler.executeStudentInfoQuery(fullInfo.Appointment.AttendeeID);
                fullInfo.Student = student;
            }

            return fullInfo;
        }


    }
}