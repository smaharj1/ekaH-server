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
    [Serializable]
    public class JokeDialog: IDialog<IMessageActivity>
    {
        

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("You called it!");

            string joke = GetChuckJoke(context);

            await context.PostAsync(joke);

            context.Done<IMessageActivity>(null);
        }

        private string GetChuckJoke(IDialogContext context)
        {
            string URL = "http://api.icndb.com/jokes/";
            const string urlParameters = "random";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var res = client.GetAsync(urlParameters).Result;

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