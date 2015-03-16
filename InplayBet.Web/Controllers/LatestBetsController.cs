

namespace InplayBet.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;

    public class LatestBetsController : BaseController
    {
        private readonly IBetDataRepository _betDataRepository;
        private readonly int _defaultWiningBetPegSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="RankingController"/> class.
        /// </summary>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public LatestBetsController(IBetDataRepository betDataRepository)
        {
            this._betDataRepository = betDataRepository;
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
                        x => x.CreatedOn, true).ToList();

                return View(bets);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }
    }
}
