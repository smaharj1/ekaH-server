using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Bot.Models
{
    /// <summary>
    /// This class holds the weather data structure similar to JSON format.
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// It holds the forecast.
        /// </summary>
        public Forecast forecast;

        /// <summary>
        /// It holds the observations received from the web call.
        /// </summary>
        public Observation current_observation;
    }

    /// <summary>
    /// This class details on the forecast of the weather.
    /// </summary>
    public class Forecast
    {
        /// <summary>
        /// It contains a simple forcast information.
        /// </summary>
        public SimpleForecast simpleforecast;
    }

    /// <summary>
    /// This class holds the simple forcast information.
    /// </summary>
    public class SimpleForecast
    {
        /// <summary>
        /// It contains the forecast info for various days.
        /// </summary>
        public ForecastDay [] forecastday;
    }

    /// <summary>
    /// This class contains the forecast information of a single day.
    /// </summary>
    public class ForecastDay
    {
        public ForecastDate date;
        public Temperature high;
        public Temperature low;
        public string conditions;
        public string icon;
        public string icon_url;
        public int avehumidity;
        public SnowInfo snow_allday;
    }

    /// <summary>
    /// This class contains text representation of the forecast.
    /// </summary>
    public class ForecastDate
    {
        public string pretty;
        public int day;
        public int month;
        public int year;
    }

    /// <summary>
    /// This class contains various forms of temperature.
    /// </summary>
    public class Temperature
    {
        public float fahrenheit;
        public float celcius;
    }

    /// <summary>
    /// This class contains the snow info in centimeters.
    /// </summary>
    public class SnowInfo
    {
        public float cm;
    }

    /// <summary>
    /// This class holds the observations of the weather.
    /// </summary>
    public class Observation
    {
        public string temperature_string;
        public string weather;
    }
}