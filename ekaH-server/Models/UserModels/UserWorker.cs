using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ekaH_server.Models.UserModels
{
    public class UserWorker
    {
        private UserWorker()
        {

        }

        public static FacultyInfo extractFacultyInfo(MySqlDataReader reader)
        {
            if (reader.Read())
            {
                // The user exists. Now read the line and extract the information into an object.
                FacultyInfo facultyMem = new FacultyInfo();
                facultyMem.FirstName = reader.GetString(0);
                facultyMem.LastName = reader.GetString(1); 
                facultyMem.Email = reader.GetString(2); 
                facultyMem.Department = reader.GetString(3);
                facultyMem.Education = getStringSafe(reader, 4);
                facultyMem.University = getStringSafe(reader, 5);
                facultyMem.Concentration = getStringSafe(reader, 6);

                Address address = new Address();
                address.StreetAdd1 = getStringSafe(reader, 7);
                address.StreetAdd2 = getStringSafe(reader, 8);
                address.City = getStringSafe(reader, 9);
                address.State = getStringSafe(reader, 10);
                address.Zip = getStringSafe(reader, 11);
                facultyMem.Address = address;

                facultyMem.Phone = getStringSafe(reader, 12);

                return facultyMem;
            }
            else
            {
                // The faculty member does not exist. So, give an error.
                return null;
            }
        }

        public static string getStringSafe(MySqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetString(index);
            }
            else
            {
                return "";
            }
        }


        // TODO: NEED TO WORK ON THIS
        public static StudentInfo extractStudentInfo(MySqlDataReader dataReader)
        {
            StudentInfo infoReceived = null;
            // Handle the dataReader to read the data from the database and store it in StudentInfo class.
            
            infoReceived = new StudentInfo();
            infoReceived.Email = getStringSafe(dataReader, 2);
            infoReceived.FirstName = getStringSafe(dataReader, 0);
            infoReceived.LastName = getStringSafe(dataReader, 1);
            infoReceived.Education = getStringSafe(dataReader, 3);
            infoReceived.Concentration = getStringSafe(dataReader, 4);
            infoReceived.Graduation = int.Parse(getStringSafe(dataReader, 5));

            Address addr = new Address();
            addr.StreetAdd1 = getStringSafe(dataReader, 6);
            addr.StreetAdd2 = getStringSafe(dataReader, 7);
            addr.City = getStringSafe(dataReader, 8);
            addr.State = getStringSafe(dataReader, 9);
            addr.Zip = getStringSafe(dataReader, 10);
            infoReceived.Address = addr;
            infoReceived.Phone = getStringSafe(dataReader, 11);

            
            return infoReceived;
        }

    }
}