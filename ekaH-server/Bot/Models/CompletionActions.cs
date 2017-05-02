using ekaH_server.App_DBHandler;
using ekaH_server.Controllers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ekaH_server.Bot.Models
{
    /// <summary>
    /// This class handles the actions required while the intents handling is completed.
    /// </summary>
    public class CompletionActions
    {
        /// <summary>
        /// This class prepares the message activity when user completes the form to 
        /// find out the courses offered into a user friendly message.
        /// </summary>
        /// <param name="a_context">It holds the context.</param>
        /// <param name="a_classes">It holds the class.</param>
        /// <returns>Returns the message prepared.</returns>
        public static IMessageActivity OnViewClassCompleted(IDialogContext a_context, ViewClass a_classes)
        {
            /// Change the text for semester.
            if (a_classes.m_semester.Equals("fall", StringComparison.OrdinalIgnoreCase))
            {
                a_classes.m_semester = "F";
            }
            else
            {
                a_classes.m_semester = "S";
            }

            /// Gets the courses taught by the professor with the provided year and semester information.
            CourseController controller = new CourseController();
            List<string> courses = controller.GetCoursesByParametersInString(a_classes.m_profEmail, a_classes.m_semester, (short)a_classes.m_year);
            IMessageActivity message = a_context.MakeMessage();

            /// Adds the result into the receipt card to properly format the message.
            if (courses.Count > 0)
            {
                List<ReceiptItem> courseItems = new List<ReceiptItem>();

                foreach (string course in courses)
                {
                    courseItems.Add(new ReceiptItem(course));
                }

                ReceiptCard card = new ReceiptCard()
                {
                    Title = "All the courses",
                    Items = courseItems
                };

                message.Attachments = new List<Attachment>();
                message.Attachments.Add(card.ToAttachment());
            }
            else
            {
                message.Text = "There are no courses in your selected semester/year";
            }

            /// Returns the message.
            return message;
        }

        /// <summary>
        /// This function prepares the message when the user enters all the information about making 
        /// an appointment through a form.
        /// </summary>
        /// <param name="a_context">It holds the context.</param>
        /// <param name="a_app">It holds the appointment information.</param>
        /// <returns>Returns the message.</returns>
        public static IMessageActivity OnAppointmentCompleted(IDialogContext a_context, BotAppointment a_app)
        {
            /// Adds an appointment to the database.
            appointmentsController controller = new appointmentsController();
            appointment appoint = new appointment();
            appoint.scheduleID = a_app.m_scheduleID;
            DateTime dt = a_app.m_startDate.Date + a_app.m_startTime.TimeOfDay;
            appoint.startTime = dt;
            appoint.endTime = dt.AddMinutes(30);
            appoint.attendeeID = a_app.m_stdEmail;
            appoint.confirmed = (sbyte)0;

            controller.AddAppointment(appoint);

            /// Uses a card for showing their data.
            var resultMessage = a_context.MakeMessage();

            resultMessage.Attachments = new List<Attachment>();
            string ThankYouMessage;

            ThankYouMessage = "Thanks for the info! Your appointment is sent! \n View your confirmation through the portal! " +
                "Please make sure you have all of your questions before you visit the professor";

            /// Prepares a thumbnail card to display the result.
            ThumbnailCard thumbnailCard = new ThumbnailCard()
            {
                Title = "Appointment with " + a_app.m_professorName,
                Subtitle = "in his office on " + appoint.startTime.ToString("MM-dd-yyyy hh:mm tt"),
                Text = ThankYouMessage,
                Images = new List<CardImage>()
        {
            new CardImage() { Url = "https://upload.wikimedia.org/wikipedia/en/e/ee/Unknown-person.gif" }
        },
            };

            resultMessage.Attachments.Add(thumbnailCard.ToAttachment());

            return resultMessage;
        }
    }
}