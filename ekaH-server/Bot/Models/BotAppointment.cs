using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Bot.Models
{
    /// <summary>
    /// This class holds the model for storing the user provided information for the appointment.
    /// </summary>
    [Serializable]
    public class BotAppointment
    {
        /// <summary>
        /// It holds the professor's name after prompting it.
        /// </summary>
        [Prompt(new string[] { "What is your professor's name?" })]
        public string m_professorName { get; set; }

        /// <summary>
        /// It holds the professor's email after prompting it.
        /// </summary>
        [Prompt(new string[] { "What is your professor's email?"})]
        public string m_professorEmail { get; set; }

        /// <summary>
        /// It holds the your email after prompting it.
        /// </summary>
        [Prompt(new string[] { "What is your email?"})]
        public string m_stdEmail { get; set; }

        /// <summary>
        /// It holds the the time the user wants to meet after prompting it.
        /// </summary>
        [Prompt(new string[] { "When would you like to meet him?" })]
        [Describe("You can say today/tomorrow or specific dates")]
        public DateTime m_startDate;

        /// <summary>
        /// It holds the time after prompting it.
        /// </summary>
        [Prompt(new string[] { "What time?" })]
        public DateTime m_startTime;

        /// <summary>
        /// It holds the schedule ID of the professor.
        /// </summary>
        public int m_scheduleID;

        /// <summary>
        /// This is a constructor.
        /// </summary>
        /// <param name="a_result">It holds the luis result.</param>
        public BotAppointment(LuisResult a_result)
        {
            EntityRecommendation date;
            EntityRecommendation time;

            /// Checks if the user already entered the date and time so that bot does 
            /// not ask it again.
            a_result.TryFindEntity("builtin.datetime.date", out date);
            a_result.TryFindEntity("builtin.datetime.time", out time);

            if (date != null) m_startDate = DateTime.Parse(date.Resolution.Values.First());
            if (time != null) m_startTime = DateTime.Parse(time.Resolution.Values.First());
        }
    }
}