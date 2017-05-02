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
    /// <summary>
    /// This class helps give the weather update to the user.
    /// </summary>
    public class WeatherDialog: IDialog<IMessageActivity>
    {
        /// <summary>
        /// It holds the LUIS result that gives all the intent information.
        /// </summary>
        private LuisResult m_result;

        /// <summary>
        /// It holds the given date that needs the weather update for.
        /// </summary>
        private DateTime m_givenDate;

        public WeatherDialog(LuisResult res)
        {
            this.m_result = res;
        }

        /// <summary>
        /// This function starts once the class is initiated. It gets the weather for the desired
        /// day.
        /// </summary>
        /// <param name="a_context">It holds the context.</param>
        /// <returns></returns>
        public async Task StartAsync(IDialogContext a_context)
        {
            await a_context.PostAsync("Here is your weather report");

            EntityRecommendation date;

            /// Checks if the user has mentioned what day. If not, then just give the weather update
            /// for today.
            m_result.TryFindEntity("builtin.datetime.date", out date);
            if (date != null)
            {
                m_givenDate = DateTime.Parse(date.Resolution.Values.First());
            }
            else
            {
                m_givenDate = DateTime.Today;
            }

            /// Gets the string representation of weather by making web call.
            string answerText = GetWeather(a_context, m_givenDate);

            await a_context.PostAsync(answerText);

            a_context.Done<IMessageActivity>(null);
        }

        /// <summary>
        /// This function gets the weather and parses it into a user friendly string
        /// representation of the weather.
        /// </summary>
        /// <param name="a_context">It holds the context.</param>
        /// <param name="a_date">It holds the date.</param>
        /// <returns>Returns the string representation of the weather.</returns>
        private string GetWeather(IDialogContext a_context, DateTime a_date)
        {
            /// It holds the URL to make web call to get weather data.
            string URL = "http://api.wunderground.com/api/8d661171e45c31fb/";
            const string urlParameters = "forecast10day/conditions/q/autoip.json";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            /// Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            string ans = "";

            try
            {
                var res = client.GetAsync(urlParameters).Result;
                
                /// Gets the weather data.
                if (res.IsSuccessStatusCode)
                {
                    Weather weather = res.Content.ReadAsAsync<Weather>().Result;
                    
                    /// Checks the date for weather and builds a string likewise.
                    if (a_date == DateTime.Today)
                    {
                        ans = "The current weather is: \n "
                            + weather.current_observation.weather + " and temperature is " + weather.current_observation.temperature_string;
                    }
                    else
                    {
                        foreach(ForecastDay forecastDay in weather.forecast.simpleforecast.forecastday)
                        {
                            DateTime temp = new DateTime(forecastDay.date.year, forecastDay.date.month, forecastDay.date.day);

                            if (temp.Date == a_date.Date)
                            {
                                ans = "The weather for " + a_date.ToString() + " is as follows: \n " +
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