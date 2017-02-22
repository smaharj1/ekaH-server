using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ekaH_server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // TODO: Maybe do the database connection here.
            
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
