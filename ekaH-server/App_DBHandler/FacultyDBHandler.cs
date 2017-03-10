using ekaH_server.Models;
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

        public static StudentInfo executeStudentInfoQuery(string emailID)
        {
            DBConnection db = DBConnection.getInstance();

            MySqlDataReader dataReader = null;

            string reqQuery = "select * from student_info where email='" + emailID + "';";

            try
            {
                MySqlCommand cmd = new MySqlCommand(reqQuery, db.getConnection());
                dataReader = cmd.ExecuteReader();
            }
            catch (MySqlException)
            {
                throw new Exception();
            }

            StudentInfo infoReceived = null;
            // Handle the dataReader to read the data from the database and store it in StudentInfo class.
            if (dataReader.Read())
            {
                infoReceived = new StudentInfo();
                infoReceived.Email = emailID;
                infoReceived.FirstName = UserWorker.getStringSafe(dataReader, 0);
                infoReceived.LastName = UserWorker.getStringSafe(dataReader, 1);
                infoReceived.Education = UserWorker.getStringSafe(dataReader, 3);
                infoReceived.Concentration = UserWorker.getStringSafe(dataReader, 4);
                infoReceived.Graduation = int.Parse(UserWorker.getStringSafe(dataReader, 5));

                Address addr = new Address();
                addr.StreetAdd1 = UserWorker.getStringSafe(dataReader, 6);
                addr.StreetAdd2 = UserWorker.getStringSafe(dataReader, 7);
                addr.City = UserWorker.getStringSafe(dataReader, 8);
                addr.State = UserWorker.getStringSafe(dataReader, 9);
                addr.Zip = UserWorker.getStringSafe(dataReader, 10);
                infoReceived.Address = addr;
                infoReceived.Phone = UserWorker.getStringSafe(dataReader, 11);

            }

            dataReader.Dispose();
            return infoReceived;
            
        }


        public static bool executePutStudentInfo(StudentInfo student)
        {
            DBConnection db = DBConnection.getInstance();
            Address address = student.Address == null ? new Address() : student.Address;

            string reqQuery = "update student_info set firstName = '" + student.FirstName + "', lastName = '" + student.LastName + "', graduationYear = " +
                student.Graduation + ", education = '" + student.Education +
                "', concentration = '" + student.Concentration + "', streetAdd1 = '" + address.StreetAdd1 +
                "', streetAdd2 = '" + address.StreetAdd2 + "', state = '" + address.State + "', city ='" + address.City + "', zip = '" +
                address.Zip + "', phone='" + student.Phone + "' where email = '" + student.Email + "';";

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

        public static FacultyInfo executeFacultyInfoQuery(string emailID)
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
                throw new Exception();
            }

            FacultyInfo faculty = UserWorker.extractFacultyInfo(dataReader);

            dataReader.Dispose();
            return faculty;
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