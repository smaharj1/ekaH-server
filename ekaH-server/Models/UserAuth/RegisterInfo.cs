using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models.UserAuth
{
    /// <summary>
    /// This class holds the information for registration of the student.
    /// It does not follow "m_" to follow the json format.
    /// </summary>
    public class RegisterInfo
    {
        public Boolean isStudent { get; set; }
        public string userEmail { get; set; }
        public string pswd { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string extraInfo { get; set; }
    }
}