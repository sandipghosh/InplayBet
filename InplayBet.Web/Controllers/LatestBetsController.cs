

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using System.Web.Mvc;

    public class LatestBetsController : BaseController
    {
        //
        // GET: /LatestBets/

        public ActionResult Index()
        {
            return View();
        }

    }
}
