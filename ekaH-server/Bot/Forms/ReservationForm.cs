﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ekaH_server.Bot.Models;


namespace ekaH_server.Bot.Forms
{
    [Serializable]
    public class ReservationForm
    {
        public static IForm<Reservation> BuildForm()
        {
            return new FormBuilder<Reservation>()
                .Field(nameof(Reservation.Name))
                .Field(nameof(Reservation.Email), validate: ValidateContactInformation)
                .Field(nameof(Reservation.PhoneNumber))
                .Field(nameof(Reservation.ReservationDate))
                .Field(new FieldReflector<Reservation>(nameof(Reservation.ReservationTime))
                    .SetPrompt(PerLinePromptAttribute("What time would you like to arrive?"))
                    ).AddRemainingFields()
                .Build();
        }

        private static Task<ValidateResult> ValidateContactInformation(Reservation state, object response)
        {
            var result = new ValidateResult();
            string contactInfo = string.Empty;
            if (GetEmailAddress((string)response, out contactInfo))
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

        public static bool GetEmailAddress(string response, out string contactInfo)
        {
            contactInfo = string.Empty;
            var match = Regex.Match(response, @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (match.Success)
            {
                contactInfo = match.Value;
                return true;
            }
            return false;
        }

        private static PromptAttribute PerLinePromptAttribute(string pattern)
        {
            return new PromptAttribute(pattern)
            {
                ChoiceStyle = ChoiceStyleOptions.PerLine
            };
        }


    }
}