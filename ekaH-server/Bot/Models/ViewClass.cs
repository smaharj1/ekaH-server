using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Bot.Models
{
    [Serializable]
    public class ViewClass
    {
        [Prompt(new string[] { "What is the Professor's name?"})]
        public string ProfName { get; set; }

        [Prompt(new string[] { "What is the Professor's email?" })]
        public string ProfEmail { get; set; }

        [Prompt(new string[] { "Which semester are you looking for?" })]
        public string Semester { get; set; }

        [Prompt(new string[] { "Last question, what year?" })]
        [Numeric(1500, 9999)]
        public int? Year;

        public ViewClass()
        {

        }
        
    }
}