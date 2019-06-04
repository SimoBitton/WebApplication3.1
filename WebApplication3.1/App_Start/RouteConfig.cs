using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication3._1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "FirstMission",
                url: "display/{ip}/{port}",
                defaults: new { controller = "First", action = "Index" }
          );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "SecondMission",
              url: "display/{ip}/{port}/{time}",
              defaults: new { controller = "First", action = "Second" }
          );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "ThirdMission",
              url: "save/{ip}/{port}/{time}/{sec}/flight1",
              defaults: new { controller = "First", action = "Third" }
          );

            routes.MapRoute(
             name: "Default",
             url: "{action}/{id}",
             defaults: new { controller = "First", action = "Default", id = UrlParameter.Optional }
          );

        }
    }
}
