using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models.UserModels
{
    public class FacultyInfo : Object
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Education { get; set; }
        public Address Address { get; set; }
        public string University { get; set; }
        public string Concentration { get; set; }
        public string Phone { get; set; }
    }

    public class StudentInfo : Object
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Education { get; set; }
        public Address Address { get; set; }
        public string Concentration { get; set; }
        public int Graduation { get; set; }
        public string Phone { get; set; }
    }
}