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

namespace ekaH_server.Bot.Dialogs
{
    [Serializable]
    [LuisModel("5ffdc213-ee58-449f-8f66-194cf3a86c83", "a1c360ac2dc04fd2b266f14a57f41169")]
    public class RootDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Joke")]
        public async Task TellAJoke(IDialogContext context, LuisResult result)
        {
            context.Call(new JokeDialog(), ResumeAfterOptionDialog);
        }

        [LuisIntent("Weather")]
        public async Task GetWeatherReport(IDialogContext context, LuisResult result)
        {
            context.Call(new WeatherDialog(result), ResumeAfterOptionDialog);
        }

        [LuisIntent("ViewClasses")]
        public async Task GetClasses(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Let's get you started!");
                var form = new FormDialog<ViewClass>(
                    new ViewClass(),
                    ViewClassForm.BuildForm,
                    FormOptions.PromptInStart
                    );

                context.Call(form, OnViewClassCompletion);
            }
            catch(Exception ex)
            {
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("ReserveATable")]
        public async Task ReserveATable(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Great, lets book a table for you. You will need to provide a few details.");
                var form = new FormDialog<Reservation>(
                new Reservation(context.UserData.Get<String>("Name")),
                ReservationForm.BuildForm,
                FormOptions.PromptInStart,
                null);

                context.Call(form, this.ReservationFormComplete);
            }
            catch (Exception ex)
            {

                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("SayHello")]
        public async Task SayHello(IDialogContext context, LuisResult result)
        {
            context.Call(new UserInfoDialog(), this.ResumeAfterOptionDialog);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Insert Help Dialog here");
            context.Wait(MessageReceived);
        }


        private async Task ReservationFormComplete(IDialogContext context, IAwaitable<Reservation> result)
        {
            try
            {
                var reservation = await result;
                await context.PostAsync("Thanks for the using Dinner Bot.");
                //use a card for showing their data
                var resultMessage = context.MakeMessage();
                //resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();
                string ThankYouMessage;
                
                if (reservation.SpecialOccasion == Reservation.SpecialOccasionOptions.none)
                {
                    ThankYouMessage = reservation.Name + ", thank you for joining us for dinner, we look forward to having you and your guests.";
                }
                else
                {
                    ThankYouMessage = reservation.Name + ", thank you for joining us for dinner, we look forward to having you and your guests for the " + reservation.SpecialOccasion;
                }
                ThumbnailCard thumbnailCard = new ThumbnailCard()
                {

                    Title = String.Format("Dinner Reservations on {0}", reservation.ReservationDate.ToString("MM/dd/yyyy")),
                    Subtitle = String.Format("at {1} for {0} people", reservation.NumberOfDinners, reservation.ReservationTime.ToString("hh:mm")),
                    Text = ThankYouMessage,
                    Images = new List<CardImage>()
        {
            new CardImage() { Url = "https://upload.wikimedia.org/wikipedia/en/e/ee/Unknown-person.gif" }
        },
                };

                resultMessage.Attachments.Add(thumbnailCard.ToAttachment());
                await context.PostAsync(resultMessage);
            }
            catch (FormCanceledException)
            {
                await context.PostAsync("You canceled the transaction, ok. ");
            }
            catch (Exception ex)
            {
                var exDetail = ex;
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }

        private async Task OnViewClassCompletion(IDialogContext context, IAwaitable<ViewClass> result)
        {
            try
            {
                ViewClass classes = await result;

                IMessageActivity message = CompletionActions.OnViewClassCompleted(context, classes);
                await context.PostAsync(message);

            }
            catch (FormCanceledException)
            {
                await context.PostAsync("You canceled the transaction, ok. ");
            }
            catch (Exception ex)
            {
                var exDetail = ex;
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }


    }
}