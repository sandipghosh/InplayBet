

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using System.Web.Mvc;

    public class CommonController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
