using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    public class LogInInfo
    {
        public Boolean isStudent { get; set; }
        public string userEmail { get; set; }
        public string pswd { get; set; }


        /*
        public LogInInfo(string email, string password, Boolean isStd)
        {
            isStudent = isStd;
            userEmail = email;
            pswd = password;
        }
        */
        
        

    }
}