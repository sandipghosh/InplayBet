﻿

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class LatestBetsController : BaseController
    {
        private readonly IBetDataRepository _betDataRepository;
        private readonly IUserDataRepository _userDataRepository;
        private readonly IChallengeDataRepository _challengeDataRepository;
        private readonly int _defaultWiningBetPegSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="RankingController"/> class.
        /// </summary>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public LatestBetsController(IBetDataRepository betDataRepository,
            IUserDataRepository userDataRepository,
            IChallengeDataRepository challengeDataRepository)
        {
            this._betDataRepository = betDataRepository;
            this._userDataRepository = userDataRepository;
            this._challengeDataRepository = challengeDataRepository;
            this._defaultWiningBetPegSize = CommonUtility.GetConfigData<int>("DefaultWiningBetPegSize");
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
                List<BetModel> bets = this._betDataRepository
                    .GetList(1, this._defaultWiningBetPegSize,
                        x => x.StatusId.Equals((int)StatusCode.Active),
                        x => x.CreatedOn, false).ToList();


                if (!bets.IsEmptyCollection())
                {
                    bets.ForEach(x =>
                    {
                        x.Challenge = this._challengeDataRepository.Get(x.ChallengeId);
                        if (x.Challenge != null) x.Challenge.User = this._userDataRepository.Get(x.Challenge.UserKey);
                    });
                }
                return View(bets);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult GoToProfile(int userKey)
        {
            try
            {
                UserModel user = this._userDataRepository.Get(userKey);
                if (user != null)
                {
                    string profileUrl = Url.Action("ViewProfile", "MemberProfile", new { ares = "", userId = user.UserId });
                    return Json(new { ProfileUrl = profileUrl.ToBase64Encode() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return null;
        }
    }
}
