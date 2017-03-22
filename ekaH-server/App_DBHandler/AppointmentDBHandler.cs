using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ekaH_server.App_DBHandler
{
    public class AppointmentDBHandler
    {
        // Returns the SQL safe date string.
        private static string getDateString(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private static string getTimeString(TimeSpan time)
        {
            return time.ToString();
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


        public static bool checkIfAppointmentExists(Appointment appointment)
        {
            DBConnection db = DBConnection.getInstance();

            string requestQuery = "select * from appointments where scheduleID = " + appointment.ScheduleID + " and startTime = '" +
                getTimeString(appointment.StartTime) + "' and endTime = '" + getTimeString(appointment.EndTime) + "';";

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

            string requestQuery = "insert into appointments(scheduleID, startTime, endTime, attendeeID) values ("+
                appointment.ScheduleID+", '"+ getTimeString(appointment.StartTime) +"','"+getTimeString(appointment.EndTime)+"','"+
                appointment.AttendeeID+"');";

            try
            {
                MySqlCommand cmd = new MySqlCommand(requestQuery, db.getConnection());
                cmd.ExecuteNonQuery();
            }
            catch(MySqlException)
            {
                throw new Exception();
            }

            return true;
        }

        public static List<Appointment> getAppointmentsByProfessorID(string email)
        {
            DBConnection db = DBConnection.getInstance();

            string requestQuery = "select * from appointments where scheduleID in (select id from officehours where professorID = '"+email+"');";
            
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
                    Appointment tempApp = new Appointment();
                    tempApp.AppointmentID = (int)reader.GetValue(0);
                    tempApp.ScheduleID = (int)reader.GetValue(1);
                    tempApp.StartTime = reader.GetTimeSpan(2);
                    tempApp.EndTime = reader.GetTimeSpan(3);
                    tempApp.AttendeeID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    tempApp.Confirmed = reader.GetBoolean(5);
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


    }
}