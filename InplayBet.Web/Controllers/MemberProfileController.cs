
namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using System.Web.Mvc;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;

    public class MemberProfileController : BaseController
    {
        private readonly IUserDataRepository _userDataRepository;

        public MemberProfileController(IUserDataRepository userDataRepository)
        {
            this._userDataRepository = userDataRepository;
        }

        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Index()
        {
            Session["USERKEY"] = 5;
            UserModel user = this._userDataRepository.Get(5);
            ViewBag.UserKey = 5;
            return View(user);
        }

    }
}
