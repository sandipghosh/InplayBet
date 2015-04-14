

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using MoreLinq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class LatestWinnersController : BaseController
    {
        private readonly IChallengeDataRepository _challengeDataRepository;
        private readonly IUserRankDataRepository _userRankDataRepository;
        private readonly int _defaultWiningMemberPegSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="LatestWinnersController"/> class.
        /// </summary>
        /// <param name="challengeDataRepository">The challenge data repository.</param>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public LatestWinnersController(IChallengeDataRepository challengeDataRepository,
            IUserRankDataRepository userRankDataRepository)
        {
            this._challengeDataRepository = challengeDataRepository;
            this._userRankDataRepository = userRankDataRepository;
            this._defaultWiningMemberPegSize = CommonUtility.GetConfigData<int>("DefaultWiningMemberPegSize");
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Index()
        {
            List<WinnerViewModel> winners = new List<WinnerViewModel>();
            try
            {
                List<ChallengeModel> challenges =
                this._challengeDataRepository.GetList(1, this._defaultWiningMemberPegSize,
                    x => x.StatusId.Equals((int)StatusCode.Active)
                        && x.ChallengeStatus.Equals(StatusCode.Won.ToString()),
                        x => x.UpdatedOn, false).ToList();

                if (!challenges.IsEmptyCollection())
                {
                    List<ChallengeModel> result = challenges
                        .GroupBy(x => x.UserKey, (key, g) => new
                        {
                            UserKey = key,
                            Challenge = g.MaxBy(m => m.UpdatedOn)
                        }).Select(y => y.Challenge).ToList<ChallengeModel>();

                    winners = result.Select(x => new WinnerViewModel
                    {
                        WonChallenge = x,
                        User = this._userRankDataRepository
                            .GetList(y => y.UserKey.Equals(x.UserKey))
                            .ToList().FirstOrDefaultCustom()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return View(winners);
        }

    }
}
