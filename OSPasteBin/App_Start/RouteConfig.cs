using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OSPasteBin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "NewPost",
                url: "New",
                defaults: new { controller = "PasteNote", action="New" }
                );

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "PasteNotes",
                url: "{action}/{id}/{mode}",
                defaults: new { controller = "PasteNote", action = "Index", id = UrlParameter.Optional, mode = UrlParameter.Optional },
                constraints: new { id = @"\d+"}
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PasteNote", action = "Index",  id = UrlParameter.Optional }
            );


        }
    }
}