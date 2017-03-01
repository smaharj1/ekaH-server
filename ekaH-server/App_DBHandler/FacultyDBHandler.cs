using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using ekaH_server.Models.UserModels;

namespace ekaH_server.App_DBHandler
{
    public class FacultyDBHandler
    {
        private FacultyDBHandler()
        {

        }

        public static MySqlDataReader executeFacultyInfoQuery(string emailID)
        {
            DBConnection db = DBConnection.getInstance();

            MySqlDataReader dataReader = null;

            string reqQuery = "select * from professor_info where email='" + emailID + "';";

            //string response;
            //ErrorList result;

            // MessageBox.Show(reqQuery);
            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                dataReader = cmd.ExecuteReader();

                
            }
            catch (MySqlException)
            {
                return null;
            }

            return dataReader;
        }

        public static bool executePutFacultyInfo(FacultyInfo facultyMem)
        {

            DBConnection db = DBConnection.getInstance();

            string reqQuery = "update professor_info set firstName = '" + facultyMem.FirstName + "', lastName = '" + facultyMem.LastName + "', department = '" + 
                facultyMem.Department + "', education = '" + facultyMem.Education +
                "', university = '" + facultyMem.University + "', concentration = '" + facultyMem.Concentration + "', streetAdd1 = '" + facultyMem.StreetAdd1 + 
                "', streetAdd2 = '" + facultyMem.StreetAdd2 + "', state = '" + facultyMem.State + "', zip = '" + facultyMem.Zip + "' where email = '" + facultyMem.Email + "';";

            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                cmd.ExecuteNonQuery();               
            }
            catch (MySqlException)
            {
                return false;
            }

            return true;
        }


    }
}