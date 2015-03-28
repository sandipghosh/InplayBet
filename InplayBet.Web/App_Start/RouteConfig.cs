using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InplayBet.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute("ActivateUser", "RegisterUser/ActivateUser/{timestamp}/{userkey}",
                new { controller = "RegisterUser", action = "ActivateUser", timestamp = UrlParameter.Optional, userkey=UrlParameter.Optional });

            routes.MapRoute("ViewProfile", "MemberProfile/ViewProfile/{userId}",
                new { controller = "MemberProfile", action = "ViewProfile" });

            routes.MapRoute("FuckYou", "Base/FuckYou/{status}",
                new { controller = "Base", action = "FuckYou" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}