using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ekaH_server
{
    /// <summary>
    /// This class helps configure the web API.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// This function registers the configurations and handles the effective routing.
        /// </summary>
        /// <param name="a_config">It holds the http configuration.</param>
        public static void Register(HttpConfiguration a_config)
        {
            /// Web API configuration and services

            /// Web API routes
            a_config.MapHttpAttributeRoutes();

            /// Routes the discussion URI to the discussion controller.
            a_config.Routes.MapHttpRoute(
                name: "discussionAPI",
                routeTemplate: "ekah/discussion/{action}/{id}",
                defaults: new { controller = "Discussion", id = RouteParameter.Optional }
                );

            /// Routes the  URI to the assignment controller.
            a_config.Routes.MapHttpRoute(
                name: "assignmentAPI",
                routeTemplate: "ekah/assignments/{action}/{id}",
                defaults: new { controller = "assignment", id = RouteParameter.Optional }
                );

            /// Routes the  URI to the appointments controller.
            a_config.Routes.MapHttpRoute(
                name: "appointmentAPI",
                routeTemplate: "ekah/appointments/{action}/{id}",
                defaults: new {controller="appointments", id=RouteParameter.Optional}
                );

            /// Routes the  URI to the submissions controller.
            a_config.Routes.MapHttpRoute(
                name: "submissionIIAPI",
                routeTemplate: "ekah/submissions/{action}/{aid}/{sid}",
                defaults: new { controller = "submissions", id = RouteParameter.Optional }
            );

            /// Routes the  URI to the submission controller.
            a_config.Routes.MapHttpRoute(
                name: "submissionAPI",
                routeTemplate: "ekah/submissions/{action}/{id}/",
                defaults: new { controller = "submissions", id = RouteParameter.Optional }
            );

            /// Routes the  URI to the faculty controller.
            a_config.Routes.MapHttpRoute(
                name: "FacultyAPI",
                routeTemplate: "ekah/faculties/{id}/",
                defaults: new { controller = "faculty", id = RouteParameter.Optional }
            );

            /// Routes the  URI to the student course relation controller.
            a_config.Routes.MapHttpRoute(
                name: "StudentCourseAPI",
                routeTemplate: "ekah/students/{id}/courses/{cid}",
                defaults: new { controller = "studentCourse", cid=RouteParameter.Optional }
                );

            /// Routes the  URI to the course controller.
            a_config.Routes.MapHttpRoute(
                name: "CourseIAPI",
                routeTemplate: "ekah/courses/{cid}/{action}",
                defaults: new { controller = "course", action = RouteParameter.Optional }
                );

            /// Routes the  URI to the course controller.
            a_config.Routes.MapHttpRoute(
                name:"CourseAPI",
                routeTemplate: "ekah/courses/{cid}",
                defaults: new {controller="course", cid = RouteParameter.Optional}
                );


            /// Routes the  URI to the student controller.
            a_config.Routes.MapHttpRoute(
                name: "StudentAPI",
                routeTemplate: "ekah/students/{id}",
                defaults: new {controller = "student", id=RouteParameter.Optional}
                );

            /// Routes the  URI to the default authentication controller.
            a_config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "ekah/auth/{action}/{id}",
                defaults: new { controller = "auth", id = RouteParameter.Optional }
            );

            /// Routes the  URI to the bot controller.
            a_config.Routes.MapHttpRoute(
                name: "BotApi",
                routeTemplate: "api/messages/{id}",
                defaults: new { controller = "message", id = RouteParameter.Optional }
            );


        }
    }
}
