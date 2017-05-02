using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ekaH_server.Bot.Dialogs
{
    /// <summary>
    /// This class handles getting user's information for the rest of the conversation. 
    /// It asks the user for their name and saves it for the remainder of the conversation.
    /// </summary>
    [Serializable]
    public class UserInfoDialog : IDialog<IMessageActivity>
    {
        /// <summary>
        /// This function is an initial function that is triggered when the class is called which 
        /// initializes the process.
        /// </summary>
        /// <param name="a_context">It holds the context.</param>
        /// <returns></returns>
        public async Task StartAsync(IDialogContext a_context)
        {
            var userName = String.Empty;

            /// Greets the user
            if (!a_context.UserData.TryGetValue<string>("Name", out userName))
            {
                await a_context.PostAsync("Before we begin, we would like to know who we are talking to?");
            }

            /// Calls the respond method below.
            await Respond(a_context);

            /// Call context.Wait and set the callback method.
            a_context.Wait(MessageReceivedAsync);

        }

        /// <summary>
        /// This function handles the user's information query.
        /// If the user already has provided the name, then it simply asks how bot can help.
        /// </summary>
        /// <param name="a_context">It holds the context.</param>
        /// <returns></returns>
        private static async Task Respond(IDialogContext a_context)
        {
            /// Variable to hold user name.
            var userName = String.Empty;

            /// Check to see if we already have username stored.
            a_context.UserData.TryGetValue<string>("Name", out userName);

            /// If not, we will ask for it. 
            if (string.IsNullOrEmpty(userName))
            {
                /// We ask here but dont capture it here, we do that in the MessageRecieved Async.
                await a_context.PostAsync("What is your name?");

                /// We set a value telling us that we need to get the name out of userdata
                a_context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                /// If name was already stored we will say hi to the user.
                await a_context.PostAsync(String.Format("Hi {0}.  How can I help you today?", userName));
                a_context.UserData.SetValue<bool>("GetName", false);
            }
        }

        /// <summary>
        /// This function is a callback function when message is received from the user regarding their name.
        /// </summary>
        /// <param name="a_context">It holds the context.</param>
        /// <param name="a_argument">It holds the arguments so that the bot can detect what was provided.</param>
        /// <returns></returns>
        public async Task MessageReceivedAsync(IDialogContext a_context, IAwaitable<IMessageActivity> a_argument)
        {
            /// Variable to hold message coming in.
            try
            {
                var message = await a_argument;
                /// Variable for userName.
                var userName = String.Empty;

                /// Variable to hold whether or not we need to get name.
                var getName = false;

                a_context.UserData.TryGetValue<string>("Name", out userName);
                a_context.UserData.TryGetValue<bool>("GetName", out getName);
                
                /// If we need to get name, we go in here.
                if (getName)
                {
                    /// We get the username we stored above. and set getname to false.
                    userName = message.Text;
                    a_context.UserData.SetValue<string>("Name", userName);
                    a_context.UserData.SetValue<bool>("GetName", true);

                    a_context.Wait(MessageReceivedAsync);
                }
                await Respond(a_context);
                a_context.Done(message);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
    }
}