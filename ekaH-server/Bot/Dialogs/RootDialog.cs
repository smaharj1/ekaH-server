using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using System.Threading.Tasks;
using ekaH_server.Bot.Forms;
using ekaH_server.Bot.Models;
using ekaH_server.Controllers;
using ekaH_server.App_DBHandler;
using ekaH_server.Models;

namespace ekaH_server.Bot.Dialogs
{
    /// <summary>
    /// This class is an initial gateway to connect and handle the desired bot requests.
    /// It is also connected with the Microsoft LUIS model which gives the desired intent
    /// by parsing through user's sentences. Then, it sends the intent to this class based
    /// on the model built in LUIS.
    /// </summary>
    [Serializable]
    [LuisModel("5ffdc213-ee58-449f-8f66-194cf3a86c83", "a1c360ac2dc04fd2b266f14a57f41169")]
    public class RootDialog : LuisDialog<object>
    {
        /// <summary>
        /// This function handles an empty intent. It is when the bot does not contain the 
        /// functionality that the user wants.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext a_context, LuisResult a_result)
        {
            /// Gives a sorry message to the user saying the bot did not understand the query.
            string message = $"Sorry, I did not understand '{a_result.Query}'";
            await a_context.PostAsync(message);
            a_context.Wait(MessageReceived);
        }

        /// <summary>
        /// This function tells a joke to the user if the user is feeling low or asks the bot
        /// to tell a joke.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        [LuisIntent("Joke")]
        public async Task TellAJoke(IDialogContext a_context, LuisResult a_result)
        {
            a_context.Call(new JokeDialog(), ResumeAfterOptionDialog);
        }

        /// <summary>
        /// This functions gets the weather update to the user as prompted by the user.
        /// If the user prompts if its rainy or sunny, it handles it accordingly by
        /// getting the information from LuisResult.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        [LuisIntent("Weather")]
        public async Task GetWeatherReport(IDialogContext a_context, LuisResult a_result)
        {
            a_context.Call(new WeatherDialog(a_result), ResumeAfterOptionDialog);
        }

        /// <summary>
        /// This function sets an appointment of the user with their professor. This feature is
        /// dependent on ekah's core application even though bot works independent for most often.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        [LuisIntent("SetAppointment")]
        public async Task SetAppointment(IDialogContext a_context, LuisResult a_result)
        {
            try
            {
                /// Initiates the form to ask required information from the user and
                /// then sets the appointment.
                await a_context.PostAsync("Alright! Let's get you started!");
                var form = new FormDialog<BotAppointment>(
                    new BotAppointment(a_result),
                    AppointmentForm.BuildForm,
                    FormOptions.PromptInStart
                    );

                a_context.Call(form, OnAppointmentCompletion);
            }
            catch (Exception ex)
            {
                await a_context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
                a_context.Wait(MessageReceived);
            }
        }

        /// <summary>
        /// This function returns all the classes that the professor is teaching for the year/semester 
        /// that the user wants to know about.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        [LuisIntent("ViewClasses")]
        public async Task GetClasses(IDialogContext a_context, LuisResult a_result)
        {
            try
            {
                /// Initiates a form to get the information about the year/time user wants to 
                /// know about.
                await a_context.PostAsync("Let's get you started!");
                var form = new FormDialog<ViewClass>(
                    new ViewClass(),
                    ViewClassForm.BuildForm,
                    FormOptions.PromptInStart
                    );

                a_context.Call(form, OnViewClassCompletion);
            }
            catch(Exception ex)
            {
                await a_context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
                a_context.Wait(MessageReceived);
            }
        }

        /// <summary>
        /// This function is triggered when the user says hello to the bot in various ways.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        [LuisIntent("SayHello")]
        public async Task SayHello(IDialogContext a_context, LuisResult a_result)
        {
            a_context.Call(new UserInfoDialog(), this.ResumeAfterOptionDialog);
        }

        /// <summary>
        /// This is a callback function when the user's request for getting the classes offered is done.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        private async Task OnViewClassCompletion(IDialogContext a_context, IAwaitable<ViewClass> a_result)
        {
            try
            {
                /// Prepares a message to be displayed.
                ViewClass classes = await a_result;

                IMessageActivity message = CompletionActions.OnViewClassCompleted(a_context, classes);
                await a_context.PostAsync(message);

            }
            catch (FormCanceledException)
            {
                await a_context.PostAsync("You canceled the transaction, ok. ");
            }
            catch (Exception ex)
            {
                var exDetail = ex;
                await a_context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
            }
            finally
            {
                a_context.Wait(MessageReceived);
            }
        }

        /// <summary>
        /// This is a callback function when setting an appointment is complete. It then gives the user with 
        /// desired result.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        private async Task OnAppointmentCompletion(IDialogContext a_context, IAwaitable<BotAppointment> a_result)
        {
            try
            {
                /// Prepares the message to be displayed to the user.
                BotAppointment appointment = await a_result;

                IMessageActivity message = CompletionActions.OnAppointmentCompleted(a_context, appointment);
                await a_context.PostAsync(message);
            }
            catch (FormCanceledException)
            {
                await a_context.PostAsync("You canceled the transaction, ok. ");
            }
            catch (Exception ex)
            {
                var exDetail = ex;
                await a_context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
            }
            finally
            {
                a_context.Wait(MessageReceived);
            }
        }

        /// <summary>
        /// This is a general call back function when nothing needs to be done once the user's request 
        /// is complete.
        /// </summary>
        /// <param name="a_context">It holds the current context of the user.</param>
        /// <param name="a_result">It holds the LUIS result that has more information
        /// on the intents and utterances by the user.</param>
        /// <returns></returns>
        private async Task ResumeAfterOptionDialog(IDialogContext a_context, IAwaitable<object> a_result)
        {
            try
            {
                var message = await a_result;
            }
            catch (Exception ex)
            {
                await a_context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                a_context.Wait(MessageReceived);
            }
        }


    }
}