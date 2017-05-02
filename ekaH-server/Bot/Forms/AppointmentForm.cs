using System;
using Microsoft.Bot.Builder.FormFlow;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ekaH_server.Bot.Models;
using ekaH_server.App_DBHandler;
using ekaH_server.Controllers;
using System.Text.RegularExpressions;

namespace ekaH_server.Bot.Forms
{
    /// <summary>
    /// This class handles the form flow for asking all the required information from the user to create
    /// an appointment.
    /// </summary>
    [Serializable]
    public class AppointmentForm
    {
        /// <summary>
        /// It holds the two week available appointments of the professor mentioned.
        /// </summary>
        private static List<appointment> m_twoWeekSchedule = new List<appointment>();

        /// <summary>
        /// This function builds the form that sequentially asks information from the user
        /// to create an appointment.
        /// </summary>
        /// <returns>Returns a form.</returns>
        public static IForm<BotAppointment> BuildForm()
        {
            return new FormBuilder<BotAppointment>()
                .Field(nameof(BotAppointment.m_stdEmail), validate: ValidateContactInformation)
                .Field(nameof(BotAppointment.m_professorName))
                .Field(nameof(BotAppointment.m_professorEmail), validate: ValidateContactInformation)
                .Field(nameof(BotAppointment.m_startDate), validate: ValidateDateAvailable)
                .Field(nameof(BotAppointment.m_startTime), validate: ValidateTimeAvailable)
                .Build();
        }

        /// <summary>
        /// This function validates the contact information provided by the user.
        /// It validates the email address of the user.
        /// </summary>
        /// <param name="a_state">It holds the state.</param>
        /// <param name="a_response">It holds the response by the user.</param>
        /// <returns>Returns a validation result that indicates if the response is valid.</returns>
        private static Task<ValidateResult> ValidateContactInformation(BotAppointment a_state, object a_response)
        {
            var result = new ValidateResult();
            string contactInfo = string.Empty;

            /// Check if the email address is valid.
            if (GetEmailAddress((string)a_response, out contactInfo))
            {
                result.IsValid = true;
                result.Value = contactInfo;
            }
            else
            {
                result.IsValid = false;
                result.Feedback = "You did not enter valid email address.";
            }

            /// Returns the result to the user.
            return Task.FromResult(result);
        }

        /// <summary>
        /// This function validates the time provided by the user.
        /// </summary>
        /// <param name="a_state">It holds the state.</param>
        /// <param name="a_response">It holds the response by the user.</param>
        /// <returns>Returns a validation result that indicates if the response is valid.</returns>
        private static Task<ValidateResult> ValidateTimeAvailable(BotAppointment a_state, object a_response)
        {
            var result = new ValidateResult();

            DateTime datetime = a_state.m_startDate.Date;
            datetime += ((DateTime)a_response).TimeOfDay;

            /// Checks if the appointment time is available. If it is, then the response is validated.
            appointment existApp = DateTimeExists(m_twoWeekSchedule, datetime);
            if (existApp == null)
            {
                result.IsValid = false;
                result.Feedback = "There are no available office hours during that time";
            }
            else
            {
                result.IsValid = true;
                a_state.m_scheduleID = existApp.scheduleID;
                result.Value = (DateTime)a_response;
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// This function validates the date entered by the user.
        /// </summary>
        /// <param name="a_state">It holds the state.</param>
        /// <param name="a_response">It holds the response by the user.</param>
        /// <returns>Returns a validation result that indicates if the response is valid.</returns>
        private static Task<ValidateResult> ValidateDateAvailable(BotAppointment a_state, object a_response)
        {
            var result = new ValidateResult();

            /// Reminds the user that they cannot make an appointment that is beyond 2 weeks mark.
            if ((DateTime) a_response > DateTime.Today.AddDays(14))
            {
                result.IsValid = false;
                result.Feedback = "The date must be less than 2 weeks to make an appointment";
                return Task.FromResult(result);
            }
            else if ((DateTime) a_response < DateTime.Today)
            {
                result.IsValid = false;
                result.Feedback = "I know I am a bot but I can't time travel for you";
                return Task.FromResult(result);
            }

            if (m_twoWeekSchedule.Count == 0)
            {
                appointmentsController controller = new appointmentsController();
                m_twoWeekSchedule = controller.getTwoWeekSchedule(a_state.m_professorEmail);
            }

            /// Checks with the professor's schedule if the date exists. 
            if (!DateExists(m_twoWeekSchedule, (DateTime) a_response))
            {
                result.IsValid = false;
                result.Feedback = "There are no available office hours during that time. It might have already been taken";
            }
            else
            {
                result.IsValid = true;
                result.Value = (DateTime)a_response;
            }

            return Task.FromResult(result);

        }

        /// <summary>
        /// This function checks if the date exists for the appointment.
        /// </summary>
        /// <param name="a_dates">It holds all the available dates.</param>
        /// <param name="a_date">It holds the date to have an appointment.</param>
        /// <returns>Returns true if the date exists in the available appointments.</returns>
        private static bool DateExists(List<appointment> a_dates, DateTime a_date)
        {
            return a_dates.Find(app => app.startTime.Date == a_date.Date) != null;
        }

        /// <summary>
        /// This function checks if the time exists.
        /// </summary>
        /// <param name="a_dates">It holds all the available dates.</param>
        /// <param name="a_datetime">It holds the date time of making the appointment.</param>
        /// <returns>Returns the appointment if it exists.</returns>
        private static appointment DateTimeExists(List<appointment> a_dates, DateTime a_datetime)
        {
            return a_dates.Find(app => app.startTime == a_datetime);
        }

        /// <summary>
        /// This function checks if the email address is valid.
        /// </summary>
        /// <param name="a_response">It represents the response sent by the user.</param>
        /// <param name="a_contactInfo">It holds the contact information.</param>
        /// <returns>Returns true if email format is valid.</returns>
        public static bool GetEmailAddress(string a_response, out string a_contactInfo)
        {
            a_contactInfo = string.Empty;
            var match = Regex.Match(a_response, @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (match.Success)
            {
                a_contactInfo = match.Value;
                return true;
            }
            return false;
        }
    }
}