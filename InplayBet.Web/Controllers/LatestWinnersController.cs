

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using System.Web.Mvc;

    public class LatestWinnersController : BaseController
    {
        //
        // GET: /LatestWinners/

        public ActionResult Index()
        {
            return View();
        }

    }
}
