﻿using ekaH_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using ekaH_server.Models.UserModels;
using System.Windows.Forms;

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
                //dataReader.Dispose();
                return null;
            }

            return dataReader;
        }
        
        // This method changes the faculty info
        public static bool executePutFacultyInfo(FacultyInfo facultyMem)
        {

            DBConnection db = DBConnection.getInstance();
            Address address = facultyMem.Address;

            string reqQuery = "update professor_info set firstName = '" + facultyMem.FirstName + "', lastName = '" + facultyMem.LastName + "', department = '" + 
                facultyMem.Department + "', education = '" + facultyMem.Education +
                "', university = '" + facultyMem.University + "', concentration = '" + facultyMem.Concentration + "', streetAdd1 = '" + address.StreetAdd1 + 
                "', streetAdd2 = '" + address.StreetAdd2 + "', state = '" + address.State + "', city ='"+ address.City+"', zip = '" + 
                address.Zip + "', phone='"+facultyMem.Phone+"' where email = '" + facultyMem.Email + "';";

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