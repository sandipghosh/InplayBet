

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Models;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Utilities;
    using System.Web.Mvc;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using MoreLinq;

    public class LatestWinnersController : BaseController
    {
        private readonly IChallengeDataRepository _challengeDataRepository;
        private readonly IUserRankDataRepository _userRankDataRepository;

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
        }

        public ActionResult Index()
        {
            List<WinnerViewModel> winners = new List<WinnerViewModel>();
            try
            {
                List<ChallengeModel> challenges =
                this._challengeDataRepository.GetList(1, 10, x => x.StatusId.Equals((int)StatusCode.Active)
                    && x.ChallengeStatus.Equals(StatusCode.Won.ToString()), x => x.UpdatedOn, false).ToList();

                if (!challenges.IsEmptyCollection())
                {
                    List<ChallengeModel> result =
                    challenges.GroupBy(x => x.UserKey, (key, g) => new
                    {
                        UserKey = key,
                        Challenge = g.MaxBy(m => m.UpdatedOn)
                    }).Select(y => y.Challenge).ToList<ChallengeModel>();

                    result.ForEach(x =>
                    {
                        WinnerViewModel winnerViewModel = new WinnerViewModel();
                        winnerViewModel.WonChallenge = x;
                        winnerViewModel.User = this._userRankDataRepository
                            .GetList(y => y.UserKey.Equals(x.UserKey)).FirstOrDefaultCustom();
                        winners.Add(winnerViewModel);
                    });
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
