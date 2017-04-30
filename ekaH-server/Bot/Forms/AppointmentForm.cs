using System;
using Microsoft.Bot.Builder.FormFlow;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ekaH_server.Bot.Models;
using ekaH_server.App_DBHandler;
using ekaH_server.Controllers;

namespace ekaH_server.Bot.Forms
{
    [Serializable]
    public class AppointmentForm
    {
        private static List<appointment> TwoWeekSchedule = new List<appointment>();

        public static IForm<BotAppointment> BuildForm()
        {
            return new FormBuilder<BotAppointment>()
                .Field(nameof(BotAppointment.StdEmail), validate: ValidateContactInformation)
                .Field(nameof(BotAppointment.ProfessorName))
                .Field(nameof(BotAppointment.ProfessorEmail), validate: ValidateContactInformation)
                .Field(nameof(BotAppointment.StartDate), validate: ValidateDateAvailable)
                .Field(nameof(BotAppointment.StartTime), validate: ValidateTimeAvailable)
                .Build();
        }

        private static Task<ValidateResult> ValidateContactInformation(BotAppointment state, object response)
        {
            var result = new ValidateResult();
            string contactInfo = string.Empty;
            if (ReservationForm.GetEmailAddress((string)response, out contactInfo))
            {
                result.IsValid = true;
                result.Value = contactInfo;
            }
            else
            {
                result.IsValid = false;
                result.Feedback = "You did not enter valid email address.";
            }
            return Task.FromResult(result);
        }

        private static Task<ValidateResult> ValidateTimeAvailable(BotAppointment state, object response)
        {
            var result = new ValidateResult();

            DateTime datetime = state.StartDate.Date;
            datetime += ((DateTime)response).TimeOfDay;

            appointment existApp = DateTimeExists(TwoWeekSchedule, datetime);
            if (existApp == null)
            {
                result.IsValid = false;
                result.Feedback = "There are no available office hours during that time";
            }
            else
            {
                result.IsValid = true;
                state.scheduleID = existApp.scheduleID;
                result.Value = (DateTime)response;
            }

            return Task.FromResult(result);
        }

        private static Task<ValidateResult> ValidateDateAvailable(BotAppointment state, object response)
        {
            var result = new ValidateResult();

            if ((DateTime) response > DateTime.Today.AddDays(14))
            {
                result.IsValid = false;
                result.Feedback = "The date must be less than 2 weeks to make an appointment";
                return Task.FromResult(result);
            }
            else if ((DateTime) response < DateTime.Today)
            {
                result.IsValid = false;
                result.Feedback = "I know I am a bot but I can't time travel for you";
                return Task.FromResult(result);
            }

            if (TwoWeekSchedule.Count == 0)
            {
                appointmentsController controller = new appointmentsController();
                TwoWeekSchedule = controller.getTwoWeekSchedule(state.ProfessorEmail);
            }

            if (!DateExists(TwoWeekSchedule, (DateTime) response))
            {
                result.IsValid = false;
                result.Feedback = "There are no available office hours during that time. It might have already been taken";
            }
            else
            {
                result.IsValid = true;
                result.Value = (DateTime)response;
            }

            return Task.FromResult(result);

        }

        private static bool DateExists(List<appointment> dates, DateTime date)
        {
            return dates.Find(app => app.startTime.Date == date.Date) != null;
        }

        private static appointment DateTimeExists(List<appointment> dates, DateTime datetime)
        {
            return dates.Find(app => app.startTime == datetime);
        }
    }
}