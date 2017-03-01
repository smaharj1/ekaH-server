using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models.UserModels
{
    public class FacultyInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Education { get; set; }
        public string StreetAdd1 { get; set; }
        public string StreetAdd2 { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string University { get; set; }
        public string Concentration { get; set; }
    }
}