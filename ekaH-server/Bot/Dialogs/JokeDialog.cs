using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Bot.Connector;
using ekaH_server.Bot.Models;

namespace ekaH_server.Bot.Dialogs
{
    /// <summary>
    /// This class handles the jokes intent for the user. 
    /// If the user wants to hear the jokes, it handles the situation.
    /// </summary>
    [Serializable]
    public class JokeDialog: IDialog<IMessageActivity>
    {
        /// <summary>
        /// This function starts the joke handling process.
        /// </summary>
        /// <param name="a_context">It holds the context of the user.</param>
        /// <returns></returns>
        public async Task StartAsync(IDialogContext a_context)
        {
            await a_context.PostAsync("You called it!");

            /// Gets the chuck norris joke by making an API call.
            string joke = GetChuckJoke(a_context);

            await a_context.PostAsync(joke);

            a_context.Done<IMessageActivity>(null);
        }

        /// <summary>
        /// This function gets the chuck norris joke from the web api.
        /// </summary>
        /// <param name="a_context">It holds the context of the user.</param>
        /// <returns>Returns a single joke of chuck norris.</returns>
        private string GetChuckJoke(IDialogContext a_context)
        {
            /// It is the external URL to make call to get the joke.
            string URL = "http://api.icndb.com/jokes/";
            const string urlParameters = "random";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            /// Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var res = client.GetAsync(urlParameters).Result;

                /// Returns the joke.
                if (res.IsSuccessStatusCode)
                {
                    Joke dataObjects = res.Content.ReadAsAsync<Joke>().Result;
                    return dataObjects.value.joke;
                }
                else throw new Exception();
            }
            catch (Exception)
            {
                return "Chuck Norris is sleeping right now. Jokes will be later!";
            }
        }
    }
}