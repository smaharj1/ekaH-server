using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ekaH_server.App_DBHandler
{
    public class AppointmentDBHandler
    {
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
            return schedule;
        }

        // Returns the SQL safe date string.
        private static string getDateString(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}