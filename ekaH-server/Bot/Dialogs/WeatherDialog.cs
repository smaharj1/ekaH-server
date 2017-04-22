using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace ekaH_server.Bot.Dialogs
{
    public class WeatherDialog: IDialog<IMessageActivity>
    {
        private LuisResult result;

        private DateTime givenDate;

        public WeatherDialog(LuisResult res)
        {
            this.result = res;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Here is your weather report");

            EntityRecommendation date;

            result.TryFindEntity("builtin.datetime.date", out date);
            if (date != null)
            {
                givenDate = DateTime.Parse(date.Resolution.Values.First());
            }
            else
            {
                givenDate = DateTime.Today;
            }

            string answerText = GetWeather(context, givenDate);
            

            context.Done<IMessageActivity>(null);
        }

        private string GetWeather(IDialogContext context, DateTime date)
        {
            string URL = "http://api.wunderground.com/api/8d661171e45c31fb/";
            const string urlParameters = "forecast10day/conditions/q/autoip.json";

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
                    var dataReceived = res.Content.ReadAsStringAsync().Result;
                    

                    return "Hey";
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