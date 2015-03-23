
namespace InplayBet.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;

    public class MemberProfileController : BaseController
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IUserRankDataRepository _userRankDataRepository;
        private readonly IChallengeDataRepository _challengeDataRepository;
        private readonly IBetDataRepository _betDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberProfileController" /> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        public MemberProfileController(IUserDataRepository userDataRepository,
            IUserRankDataRepository userRankDataRepository,
            IChallengeDataRepository challengeDataRepository,
            IBetDataRepository betDataRepository)
        {
            this._userDataRepository = userDataRepository;
            this._userRankDataRepository = userRankDataRepository;
            this._challengeDataRepository = challengeDataRepository;
            this._betDataRepository = betDataRepository;
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
                int userKey = SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey);
                UserRankViewModel user = this._userRankDataRepository.GetList
                    (x => x.UserKey.Equals(userKey)).FirstOrDefaultCustom();

                if (user != null)
                {
                    ViewBag.ConsicutiveWonBets = _betDataRepository.GetConsicutiveBetWins(user.UserKey);
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        /// <summary>
        /// Views the profile.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult ViewProfile(string userId)
        {
            try
            {
                UserRankViewModel user = this._userRankDataRepository.GetList
                    (x => x.UserId.Equals(userId)).FirstOrDefaultCustom();

                if (user != null)
                {
                    ViewBag.ConsicutiveWonBets = _betDataRepository.GetConsicutiveBetWins(user.UserKey);
                    SharedFunctionality shared = new SharedFunctionality();
                    return View("Index", user);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userId);
            }
            return null;
        }

        /// <summary>
        /// Resets the account.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult ResetAccount(int userKey)
        {
            try
            {
                this._userDataRepository.ResetAccount(userKey);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return RedirectToActionPermanent("Index");
        }

    }
}
