using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace ekaH_server.Bot.Dialogs
{
    /// <summary>
    /// This class handles the hello intent that the user tells to the bot.
    /// </summary>
    [Serializable]
    public class HelloDialog : IDialog<object>
    {
        /// <summary>
        /// This function asynchroniously starts this dialog responding to the user's request.
        /// </summary>
        /// <param name="a_context">It holds the user's context.</param>
        /// <returns></returns>
        public async Task StartAsync(IDialogContext a_context)
        {
            /// Gree/t the user 
            await a_context.PostAsync("Hey there, how are you?");

            a_context.Done<object>(null);
        }
    }
}