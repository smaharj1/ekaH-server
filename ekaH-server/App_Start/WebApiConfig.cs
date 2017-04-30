﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ekaH_server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "discussionAPI",
                routeTemplate: "ekah/discussion/{action}/{id}",
                defaults: new { controller = "Discussion", id = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "assignmentAPI",
                routeTemplate: "ekah/assignments/{action}/{id}",
                defaults: new { controller = "assignment", id = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "appointmentAPI",
                routeTemplate: "ekah/appointments/{action}/{id}",
                defaults: new {controller="appointments", id=RouteParameter.Optional}
                );

            config.Routes.MapHttpRoute(
                name: "submissionIIAPI",
                routeTemplate: "ekah/submissions/{action}/{aid}/{sid}",
                defaults: new { controller = "submissions", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "submissionAPI",
                routeTemplate: "ekah/submissions/{action}/{id}/",
                defaults: new { controller = "submissions", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "FacultyAPI",
                routeTemplate: "ekah/faculties/{id}/",
                defaults: new { controller = "faculty", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "StudentCourseAPI",
                routeTemplate: "ekah/students/{id}/courses/{cid}",
                defaults: new { controller = "studentCourse", cid=RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "CourseIAPI",
                routeTemplate: "ekah/courses/{cid}/{action}",
                defaults: new { controller = "course", action = RouteParameter.Optional }
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

            /*
            config.Routes.MapHttpRoute(
                name: "StudentsAPI",
                routeTemplate: "ekah/stuents/{id}",
                defaults: new { controller = "faculty", id = RouteParameter.Optional }
            );*/

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "ekah/auth/{action}/{id}",
                defaults: new { controller = "auth", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "BotApi",
                routeTemplate: "api/messages/{id}",
                defaults: new { controller = "message", id = RouteParameter.Optional }
            );


        }
    }
}
