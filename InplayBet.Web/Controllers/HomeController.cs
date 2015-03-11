

namespace InplayBet.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;

    public class HomeController : BaseController
    {
        private readonly IUserDataRepository _userDataRepository;

        public HomeController(IUserDataRepository userDataRepository)
        {
            this._userDataRepository = userDataRepository;
        }
        public ActionResult Index()
        {

            var a = this._userDataRepository.GetList(x => x.StatusId == 1).ToList();
            return View();
        }

    }
}
