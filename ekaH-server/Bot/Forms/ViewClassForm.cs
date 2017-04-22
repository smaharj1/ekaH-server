using ekaH_server.Bot.Models;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ekaH_server.Bot.Forms
{
    [Serializable]
    public class ViewClassForm
    {

        public static IForm<ViewClass> BuildForm()
        {
            return new FormBuilder<ViewClass>()
                .Field(nameof(ViewClass.ProfName))
                .Field(nameof(ViewClass.ProfEmail), validate: ValidateContactInformation)
                .Field(nameof(ViewClass.Semester))
                .Field(nameof(ViewClass.Year))
                .Build();
        }

        private static Task<ValidateResult> ValidateContactInformation(ViewClass state, object response)
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
    }
}