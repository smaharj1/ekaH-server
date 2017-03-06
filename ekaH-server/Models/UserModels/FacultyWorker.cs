using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ekaH_server.Models.UserModels
{
    public class FacultyWorker
    {
        private FacultyWorker()
        {

        }

        public static FacultyInfo extractInfo(MySqlDataReader reader)
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



    }
}