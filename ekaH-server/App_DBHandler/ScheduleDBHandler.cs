using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ekaH_server.App_DBHandler
{
    public class ScheduleDBHandler
    {
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

                        requestQuery += "insert into officehours(professorID, startDtime, endDTime) values ('" + 
                            schedule.ProfessorID +"', '" + startDTime.ToString("yyyy-MM-dd HH:mm:ss") +"', '"+ 
                            endDTime.ToString("yyyy-MM-dd HH:mm:ss") +"');";
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

        public static bool deleteScheduleFromDB(Schedule schedule)
        {
            return true;
        }
    }
}