using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models.UserAuth
{
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