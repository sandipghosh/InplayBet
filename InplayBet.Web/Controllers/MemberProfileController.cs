
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
                //UserModel user = this._userDataRepository.Get
                //    (SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey));
                int userKey = SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey);
                UserRankViewModel user = this._userRankDataRepository.GetList
                    (x => x.UserKey.Equals(userKey)).FirstOrDefaultCustom();

                if (user != null)
                {
                    //int userKey = user.UserKey;
                    //ViewBag.UserKey = user.UserKey;
                    //ViewBag.TotalWonChallengeCount = user.Wins;
                    //ViewBag.TotalLostChallengeCount = user.Losses;

                    //Func<StatusCode, int, int> calculateCount = (status, uKey) =>
                    //    this._challengeDataRepository.GetCount(x => x.UserKey.Equals(uKey)
                    //        && x.StatusId.Equals((int)StatusCode.Active)
                    //        && x.ChallengeStatus.Equals(status.ToString()));

                    //ViewBag.TotalWonChallengeCount = calculateCount(StatusCode.Won, userKey);
                    //ViewBag.TotalLostChallengeCount = calculateCount(StatusCode.Lost, userKey);

                    //ViewBag.TotalWonBetCount = this._betDataRepository.GetCount(x => x.CreatedBy.Equals(userKey)
                    //    && x.StatusId.Equals((int)StatusCode.Active) && x.BetStatus.Equals(StatusCode.Won.ToString()));

                    //ViewBag.TotalWonChallengeAmount = Convert.ToDecimal(this._challengeDataRepository.Sum(x => x.UserKey.Equals(userKey)
                    //    && x.StatusId.Equals((int)StatusCode.Active) && !x.ChallengeStatus.Equals(StatusCode.Lost.ToString()),
                    //        x => (double)x.WiningPrice));

                    //ViewBag.TotalPlacedChallengeAmount = (this._challengeDataRepository.GetCount(x => x.UserKey.Equals(userKey)
                    //    && x.StatusId.Equals((int)StatusCode.Active) && x.ChallengeStatus.Equals(StatusCode.Lost.ToString()))
                    //    * CommonUtility.GetConfigData<int>("StartingBetAmount"));

                    //ViewBag.TotalProfitAmount = ((decimal)ViewBag.TotalWonChallengeAmount - (decimal)ViewBag.TotalPlacedChallengeAmount);
                    //ViewBag.UserKey = userKey;

                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

    }
}
