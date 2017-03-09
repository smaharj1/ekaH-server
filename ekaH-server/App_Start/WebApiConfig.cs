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
                name: "FacultyAPI",
                routeTemplate: "ekah/faculties/{id}/",
                defaults: new { controller = "faculty", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "CourseStudentAPI",
                routeTemplate: "ekah/courses/{cid}/students/{sid}",
                defaults: new { controller = "courseStudent", sid=RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name:"CourseAPI",
                routeTemplate: "ekah/courses/{cid}",
                defaults: new {controller="course", cid = RouteParameter.Optional}
                );

            config.Routes.MapHttpRoute(
                name: "StudentAPI",
                routeTemplate: "ekah/students/{id}",
                defaults: new {controller = "student", id=RouteParameter.Optional}
                );

            

            config.Routes.MapHttpRoute(
                name: "StudentsAPI",
                routeTemplate: "ekah/stuents/{id}",
                defaults: new { controller = "faculty", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "ekah/auth/{action}/{id}",
                defaults: new { controller = "auth", id = RouteParameter.Optional }
            );

            
        }
    }
}
