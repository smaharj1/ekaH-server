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

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = db.getConnection();
                cmd.CommandText = "select * from student_info where email=@emailID;";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("emailID", emailID);

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

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = db.getConnection();
                cmd.CommandText = "update student_info set firstName = @FirstName, lastName = @LastName, " +
                    "graduationYear = @Graduation, education = @Education, concentration = @Concentration, streetAdd1 = @StreetAdd1, "+
                    "streetAdd2 = @StreetAdd2, state = @State, city =@City, zip =@Zip, phone=@Phone where email = @Email;";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("FirstName", student.FirstName);
                cmd.Parameters.AddWithValue("LastName", student.LastName);
                cmd.Parameters.AddWithValue("Graduation", student.Graduation);
                cmd.Parameters.AddWithValue("Education", student.Education);
                cmd.Parameters.AddWithValue("Concentration", student.Concentration);
                cmd.Parameters.AddWithValue("StreetAdd1", address.StreetAdd1);
                cmd.Parameters.AddWithValue("StreetAdd2", address.StreetAdd2);
                cmd.Parameters.AddWithValue("State", address.State);
                cmd.Parameters.AddWithValue("City", address.City);
                cmd.Parameters.AddWithValue("Zip", address.Zip);
                cmd.Parameters.AddWithValue("Phone", student.Phone);
                cmd.Parameters.AddWithValue("Email", student.Email);


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
            
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = db.getConnection();
                cmd.CommandText = "select * from professor_info where email=@emailID;";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("emailID", emailID);

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
            
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = db.getConnection();
                cmd.CommandText = "update professor_info set firstName = @FirstName, lastName = @LastName, " +
                    "department = @Department, education = @Education, university = @University, concentration = @Concentration, streetAdd1 = @StreetAdd1, " +
                    "streetAdd2 = @StreetAdd2, state = @State, city =@City, zip =@Zip, phone=@Phone where email = @Email;";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("FirstName", facultyMem.FirstName);
                cmd.Parameters.AddWithValue("LastName", facultyMem.LastName);
                cmd.Parameters.AddWithValue("Department", facultyMem.Department);
                cmd.Parameters.AddWithValue("Education", facultyMem.Education);
                cmd.Parameters.AddWithValue("University", facultyMem.University);
                cmd.Parameters.AddWithValue("Concentration", facultyMem.Concentration);
                cmd.Parameters.AddWithValue("StreetAdd1", address.StreetAdd1);
                cmd.Parameters.AddWithValue("StreetAdd2", address.StreetAdd2);
                cmd.Parameters.AddWithValue("State", address.State);
                cmd.Parameters.AddWithValue("City", address.City);
                cmd.Parameters.AddWithValue("Zip", address.Zip);
                cmd.Parameters.AddWithValue("Phone", facultyMem.Phone);
                cmd.Parameters.AddWithValue("Email", facultyMem.Email);

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