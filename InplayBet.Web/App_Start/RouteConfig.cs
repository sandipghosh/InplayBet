

namespace InplayBet.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute("Home", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("Ranking", "Ranking", new { controller = "Ranking", action = "Index" });
            routes.MapRoute("LatestBets", "LatestBets", new { controller = "LatestBets", action = "Index" });
            routes.MapRoute("LatestWinners", "LatestWinners", new { controller = "LatestWinners", action = "Index" });
            routes.MapRoute("MemberSearch", "MemberSearch", new { controller = "MemberSearch", action = "Index" });
            routes.MapRoute("Reporting", "Reporting", new { controller = "Reporting", action = "Index" });
            routes.MapRoute("RegisterUser", "RegisterUser", new { controller = "RegisterUser", action = "Index" });
            routes.MapRoute("ContactUs", "ContactUs", new { controller = "ContactUs", action = "Index" });

            routes.MapRoute("MyProfile", "MemberProfile/{userId}",
                new { controller = "MemberProfile", action = "MyProfile" });

            routes.MapRoute("ActivateUser", "RegisterUser/ActivateUser/{timestamp}/{userkey}",
                new { controller = "RegisterUser", action = "ActivateUser", timestamp = UrlParameter.Optional, userkey = UrlParameter.Optional });

            routes.MapRoute("BetInfo", "Bet/BetInfo/{userKey}/{betId}",
                new { controller = "Bet", action = "BetInfo" });

            routes.MapRoute("ViewProfile", "MemberProfile/ViewProfile/{userId}",
                new { controller = "MemberProfile", action = "ViewProfile" });

            routes.MapRoute("ResetAccount", "MemberProfile/ResetAccount/{userId}",
                new { controller = "MemberProfile", action = "ResetAccount" });

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