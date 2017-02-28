using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ekaH_server.Controllers
{
    public class userController : ApiController
    {
        // GET: ekah/users
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: ekah/users/{id}
        public string Get(string id)
        {
            return "value given " + id;
        }

        // POST: api/user
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/user/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/user/5
        public void Delete(int id)
        {
        }
    }
}
