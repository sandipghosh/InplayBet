using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InplayBet.Web.Utilities;

namespace InplayBet.Web.Controllers
{
    public class FollowController : Controller
    {
        public ActionResult Set(int followBy, int followTo)
        {
            SharedFunctionality shared = new SharedFunctionality();
            return Json(new { followCount = shared.AddFollowers(followBy, followTo) },
                JsonRequestBehavior.AllowGet);
        }
    }
}
