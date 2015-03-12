
namespace InplayBet.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Utilities;

    public class MemberProfileController : BaseController
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IChallengeDataRepository _challengeDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberProfileController" /> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        public MemberProfileController(IUserDataRepository userDataRepository,
            IChallengeDataRepository challengeDataRepository)
        {
            this._userDataRepository = userDataRepository;
            this._challengeDataRepository = challengeDataRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Index()
        {
            try
            {
                SessionVeriables.SetSessionData<int>(SessionVeriables.UserKey, 5);
                UserModel user = this._userDataRepository.Get(5);

                var test = this._challengeDataRepository.Sum(x => x.UserKey == 5 && x.StatusId == 1 && x.ChallengeStatus != "Lost",
                    x => (double)x.WiningPrice);

                ViewBag.UserKey = 5;
                return View(user);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

    }
}
