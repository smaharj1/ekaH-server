using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ekaH_server.Models;
using ekaH_server.App_DBHandler;
using ekaH_server.Models.UserAuth;

namespace ekaH_server.Controllers
{
    public class ekaHController : ApiController
    {
        // GET: api/ekaH
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ekaH/Get/5
        public LogInInfo Get(int id)
        {
            //LogInInfo linfo = new LogInInfo("smaharj1@ramapo.edu", "asdf", true);
            return null;
        }

        // GET: api/ekaH/person

        /*
         * POST: api/ekaH/login
         * It handles if the user tries to log in. If it is true, it should return true. Else,
         * return the error message.
         * */
        [ActionName("login")]
        public Dictionary<string, string> Post([FromBody] LogInInfo providedInfo)
        {
            DBConnection database = DBConnection.getInstance();
            //UserAuthentication uauth = new UserAuthentication();

            ErrorList response;

            response = UserAuthentication.verifyUserExists(database, providedInfo);

            Dictionary<string, string> res = new Dictionary<string, string>();

            if (response == ErrorList.SUCCESS)
            {
                res.Add("result", "true");
            }
            else
            {
                res.Add("result", "false");
                res.Add("message", Error.getInstance().getStringError(response));
            }

            return res;
            //return response;
        }

        [HttpPost]
        [ActionName("register")]
        public Dictionary<string, string> registerUser([FromBody] RegisterInfo providedInfo)
        {
            // Gets the database instance.
            DBConnection database = DBConnection.getInstance();

            // Creates a LogInInfo for checking if the user already exists.
            LogInInfo normalizedDetail = new LogInInfo();
            normalizedDetail.isStudent = providedInfo.isStudent;
            normalizedDetail.pswd = providedInfo.pswd;
            normalizedDetail.userEmail = providedInfo.userEmail;

            // First verify that the user is not already in the list.
            ErrorList verifyIfExists = UserAuthentication.verifyUserExists(database, normalizedDetail);

            Dictionary<string, string> res = new Dictionary<string, string>();

            if (verifyIfExists == ErrorList.LOGIN_NO_USER)
            {
                // Register the user in this case
                ErrorList registrationStatus = UserAuthentication.registerUser(database, providedInfo);

                if (registrationStatus == ErrorList.SUCCESS)
                {
                    res.Add("result", "true");
                }
                else
                {
                    res.Add("result", "false");
                }

                res.Add("message", Error.getInstance().getStringError(registrationStatus));
            }
            else
            {
                res.Add("result", "false");
                if (verifyIfExists == ErrorList.SUCCESS)
                {
                    res.Add("message", Error.getInstance().getStringError(ErrorList.REGISTER_USER_EXISTS));
                }
                else
                {
                    res.Add("message", Error.getInstance().getStringError(verifyIfExists));
                }

            }

            return res;
        }

        

        // PUT: api/ekaH/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ekaH/5
        public void Delete(int id)
        {
        }
    }
}
