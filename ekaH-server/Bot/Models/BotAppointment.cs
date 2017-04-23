using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Bot.Models
{
    [Serializable]
    public class BotAppointment
    {
        [Prompt(new string[] { "What is your professor's name?" })]
        public string ProfessorName { get; set; }

        [Prompt(new string[] { "What is your professor's email?"})]
        public string ProfessorEmail { get; set; }

        [Prompt(new string[] { "What is your email?"})]
        public string StdEmail { get; set; }

        [Prompt(new string[] { "When would you like to meet him?" })]
        [Describe("You can say today/tomorrow or specific dates")]
        public DateTime StartDate;

        [Prompt(new string[] { "What time?" })]
        public DateTime StartTime;

        public int scheduleID;


        public BotAppointment(LuisResult result)
        {
            EntityRecommendation date;
            EntityRecommendation time;

            result.TryFindEntity("builtin.datetime.date", out date);
            result.TryFindEntity("builtin.datetime.time", out time);

            if (date != null) StartDate = DateTime.Parse(date.Resolution.Values.First());
            if (time != null) StartTime = DateTime.Parse(time.Resolution.Values.First());
        }


    }
}