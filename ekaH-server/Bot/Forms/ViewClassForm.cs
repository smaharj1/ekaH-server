using ekaH_server.Bot.Models;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ekaH_server.Bot.Forms
{
    /// <summary>
    /// This class helps the user fill in the information in order to find
    /// the courses offered.
    /// </summary>
    [Serializable]
    public class ViewClassForm
    {
        /// <summary>
        /// This function builds a form in order to get user's input.
        /// </summary>
        /// <returns>Returns the form.</returns>
        public static IForm<ViewClass> BuildForm()
        {
            return new FormBuilder<ViewClass>()
                .Field(nameof(ViewClass.m_profName))
                .Field(nameof(ViewClass.m_profEmail), validate: ValidateContactInformation)
                .Field(nameof(ViewClass.m_semester))
                .Field(nameof(ViewClass.m_year))
                .Build();
        }

        /// <summary>
        /// This function validates the contact information of the user (email).
        /// </summary>
        /// <param name="a_state">It holds the state.</param>
        /// <param name="a_response">It holds the response by the user.</param>
        /// <returns>Returns a validation result that indicates if the response is valid.</returns>
        private static Task<ValidateResult> ValidateContactInformation(ViewClass a_state, object a_response)
        {
            var result = new ValidateResult();
            string contactInfo = string.Empty;
            if (AppointmentForm.GetEmailAddress((string)a_response, out contactInfo))
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