using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Bot.Models
{
    /// <summary>
    /// This class is used in a form to get the information from the user to check
    /// courses offered during that time.
    /// </summary>
    [Serializable]
    public class ViewClass
    {
        /// <summary>
        /// It holds the name of the professor.
        /// </summary>
        [Prompt(new string[] { "What is the Professor's name?"})]
        public string m_profName { get; set; }

        /// <summary>
        /// It holds the email of the professor.
        /// </summary>
        [Prompt(new string[] { "What is the Professor's email?" })]
        public string m_profEmail { get; set; }

        /// <summary>
        /// It holds the semester to look for.
        /// </summary>
        [Prompt(new string[] { "Which semester are you looking for?" })]
        public string m_semester { get; set; }

        /// <summary>
        /// It holds the year to look for.
        /// </summary>
        [Prompt(new string[] { "Last question, what year?" })]
        [Numeric(1500, 9999)]
        public int? m_year;
        
    }
}