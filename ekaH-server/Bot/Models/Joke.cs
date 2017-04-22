using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Bot.Models
{
    public class Joke
    {
        public string type;
        public JokeValue value;
    }

    public class JokeValue
    {
        public int id;
        public string joke; 
    }
}