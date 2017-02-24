using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{

    public enum ErrorList
    {
        LOGIN_NO_USER,
        LOGIN_WRONG_PASSWORD,
        DATABASE_EXCEPTION,
        SUCCESS
    }

    public class Error
    {
        private static Error error;

        // This dictionary maps the error enumerator to the description of the error.
        private Dictionary<ErrorList, string> errorDict;

        private Error()
        {
            populateMap();
        }

        private void populateMap()
        {
            errorDict = new Dictionary<ErrorList, string>();
            errorDict.Add(ErrorList.LOGIN_NO_USER, "User was not found in the database \n Please enter again");
            errorDict.Add(ErrorList.LOGIN_WRONG_PASSWORD, "The password entered is wrong");
            errorDict.Add(ErrorList.DATABASE_EXCEPTION, "Database seems to be givin some problem");
        }

        public static Error getInstance()
        {
            if (error != null)
            {
                return error;
            }
            else
            {
                error = new Error();
                return error;
            }
        }

        public string getStringError(ErrorList errorCode)
        {
            return errorDict[errorCode];
        }

    }
}