

namespace InplayBet.Web.Controllers
{
    using System.Web.Mvc;
    using InplayBet.Web.Controllers.Base;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //var a = CommonUtility.GenarateRandomString();
            return View();
        }

    }
}
