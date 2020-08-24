using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace testowo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                      name: "CourseDetails",
                      url: "Course-{id}.html",
                      defaults: new { controller = "Course", action = "Details" });

            routes.MapRoute(
                      name: "CourseList",
                      url: "Category/{nameCat}.html",
                      defaults: new { controller = "Course", action = "List" });

            routes.MapRoute(
                       name: "StaticPages",
                       url: "page/{name}.html",
                       defaults: new { controller = "Home", action = "StaticPages" });
        
        routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        
    }
}
}
