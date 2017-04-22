using ekaH_server.Bot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            await context.PostAsync(answerText);

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

            string ans = "";

            try
            {
                var res = client.GetAsync(urlParameters).Result;

                if (res.IsSuccessStatusCode)
                {
                    Weather weather = res.Content.ReadAsAsync<Weather>().Result;
                    
                    if (date == DateTime.Today)
                    {
                        ans = "The current weather is: \n "
                            + weather.current_observation.weather + " and temperature is " + weather.current_observation.temperature_string;
                    }
                    else
                    {
                        foreach(ForecastDay forecastDay in weather.forecast.simpleforecast.forecastday)
                        {
                            DateTime temp = new DateTime(forecastDay.date.year, forecastDay.date.month, forecastDay.date.day);

                            if (temp.Date == date.Date)
                            {
                                ans = "The weather for " + date.ToString() + " is as follows: \n " +
                                    "It is supposed to be " + forecastDay.conditions + " \n" +
                                    "with the high of " + forecastDay.high.fahrenheit + " and low of " + forecastDay.low.fahrenheit;
                                break;
                            }
                            else continue;
                        }
                    }
                }
                else throw new Exception();
            }
            catch (Exception e)
            {
                return "Weather tower is not functioning right now. Try again later.";
            }

            return ans;


        }
    }
}