using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ekaH_server.Models;
using ekaH_server.App_DBHandler;

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


        // POST: api/ekaH/login
        [ActionName("login")]
        public string Post([FromBody] LogInInfo providedInfo)
        {

            UserAuthentication uAuth = new UserAuthentication();

            return "hello";
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
