using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Bot.Models
{
    public class Weather
    {
        public Forecast forecast;
        public Observation current_observation;
    }

    public class Forecast
    {
        public SimpleForecast simpleforecast;
    }

    public class SimpleForecast
    {
        public ForecastDay [] forecastday;
    }

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

    public class ForecastDate
    {
        public string pretty;
        public int day;
        public int month;
        public int year;
    }

    public class Temperature
    {
        public float fahrenheit;
        public float celcius;
    }

    public class SnowInfo
    {
        public float cm;
    }

    public class Observation
    {
        public string temperature_string;
        public string weather;

    }
}