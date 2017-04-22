using ekaH_server.Controllers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
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

            List<string> courses = CourseController.GetCoursesByParametersInString(classes.ProfEmail, classes.Semester, (short)classes.Year);
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
    }
}