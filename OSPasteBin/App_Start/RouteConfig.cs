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

            // New Post Route
            routes.MapRoute(
                name: "NewPost",
                url: "New",
                defaults: new { controller = "PasteNote", action = "New" }
            );

            // Account Management Route
            routes.MapRoute(
                name: "Account",
                url: "Account/{action}",
                defaults: new { controller = "Account", action = "Login" }
            );

            // New Posting Page should be the Default
            // http://site/Notes/Tag/KeyWord - Notes Tagged with Keyword
            routes.MapRoute(
                name: "PasteNotesSearch",
                url: "Notes/Tag/{keyword}",
                defaults: new { controller = "PasteNote", action = "TaggedNotes" },
                constraints: new { keyword = @"[a-zA-Z0-9]+" }
            );

            // New Posting Page should be the Default
            // http://site/Notes/id - Display note with id
            // http://site/Notes/id/Raw - Display "Raw" View of note with id
            routes.MapRoute(
                name: "PasteNotes",
                url: "{action}/{id}/{mode}",
                defaults: new { controller = "PasteNote", action = "New", id = UrlParameter.Optional, mode = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            // Default Routes
            // New Posting Page should be the Default
            // - http://site/
            // - http://site/PasteNote/New
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PasteNote", action = "New", id = UrlParameter.Optional }
            );
        }
    }
}