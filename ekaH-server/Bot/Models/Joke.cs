using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Bot.Models
{
    /// <summary>
    /// This class is data structure for a joke.
    /// </summary>
    public class Joke
    {
        /// <summary>
        /// It holds the type.
        /// </summary>
        public string type;

        /// <summary>
        /// It holds the value of the type.
        /// </summary>
        public JokeValue value;
    }

    /// <summary>
    /// This class is data structure of a joke value.
    /// </summary>
    public class JokeValue
    {
        /// <summary>
        /// It holds the id of joke.
        /// </summary>
        public int id;

        /// <summary>
        /// It holds the actual joke.
        /// </summary>
        public string joke; 
    }
}