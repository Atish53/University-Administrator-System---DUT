﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DUTAdmin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "StudentCreate",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Student", action = "CreateStudent", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "StudentDetails",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Student", action = "StudentDetails", id = UrlParameter.Optional }
           );
        }
    }
}