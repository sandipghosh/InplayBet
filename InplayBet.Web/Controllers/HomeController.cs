﻿

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
