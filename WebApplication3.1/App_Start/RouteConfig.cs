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
        // map incoming browser requests to controller action methods.
        public static void RegisterRoutes(RouteCollection routes)
        {
            //map to controller mission1 or mission4 - due to same types of parameters.
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "FirstOrFourth",
                url: "display/{ip}/{port}",
                defaults: new { controller = "First", action = "FirstOrFourth" }
          );
            //map to controller of mission2
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "SecondMission",
              url: "display/{ip}/{port}/{time}",
              defaults: new { controller = "First", action = "Second" }
          );

            //map to controller of mission3

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "ThirdMission",
              url: "save/{ip}/{port}/{time}/{sec}/{fileName}",
              defaults: new { controller = "First", action = "Third" }
          );
            //map to controller of Deafault browsing - deafult request

            routes.MapRoute(
             name: "Default",
             url: "{action}/{id}",
             defaults: new { controller = "First", action = "Default", id = UrlParameter.Optional }
          );

        }
    }
}
