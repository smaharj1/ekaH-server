using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    public class LogInInfo
    {
        public Boolean IsStudent { get; set; }
        public string UserEmail { get; set; }
        public string Pswd { get; set; }


        /*
        public LogInInfo(string email, string password, Boolean isStd)
        {
            IsStudent = isStd;
            UserEmail = email;
            Pswd = password;
        }
        */
        
        

    }
}