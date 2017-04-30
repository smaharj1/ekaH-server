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
    public class CompletionActions
    {
        public static IMessageActivity OnViewClassCompleted(IDialogContext context, ViewClass classes)
        {
            if (classes.Semester.Equals("fall", StringComparison.OrdinalIgnoreCase))
            {
                classes.Semester = "F";
            }
            else
            {
                classes.Semester = "S";
            }

            CourseController controller = new CourseController();
            List<string> courses = controller.GetCoursesByParametersInString(classes.ProfEmail, classes.Semester, (short)classes.Year);
            IMessageActivity message = context.MakeMessage();

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

            return message;
        }

        public static IMessageActivity OnAppointmentCompleted(IDialogContext context, BotAppointment app)
        {
            appointmentsController controller = new appointmentsController();
            appointment appoint = new appointment();
            appoint.scheduleID = app.scheduleID;
            DateTime dt = app.StartDate.Date + app.StartTime.TimeOfDay;
            appoint.startTime = dt;
            appoint.endTime = dt.AddMinutes(30);
            appoint.attendeeID = app.StdEmail;
            appoint.confirmed = (sbyte)0;

            controller.AddAppointment(appoint);

            //use a card for showing their data
            var resultMessage = context.MakeMessage();
            //resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            resultMessage.Attachments = new List<Attachment>();
            string ThankYouMessage;

            ThankYouMessage = "Thanks for the info! Your appointment is sent! \n View your confirmation through the portal! " +
                "Please make sure you have all of your questions before you visit the professor";

            ThumbnailCard thumbnailCard = new ThumbnailCard()
            {
                Title = "Appointment with " + app.ProfessorName,
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