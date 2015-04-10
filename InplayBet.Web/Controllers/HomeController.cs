

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        private readonly IUserRankDataRepository _userRankDataRepository;
        private readonly int _defaultLeaderboardPegSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public HomeController(IUserRankDataRepository userRankDataRepository)
        {
            this._userRankDataRepository = userRankDataRepository;
            this._defaultLeaderboardPegSize = CommonUtility.GetConfigData<int>("DefaultLeaderboardPegSize");
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
                List<UserRankViewModel> user = new List<UserRankViewModel>();
                int loggedInUser = SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey);

                if (loggedInUser > 0)
                {
                    user = this._userRankDataRepository
                   .GetList(1, this._defaultLeaderboardPegSize,
                       x => /*!string.IsNullOrEmpty(x.WinningBets) &&*/ x.UserKey != loggedInUser, x => x.Rank, true).ToList();
                }
                else
                {
                    user = this._userRankDataRepository
                   .GetList(1, this._defaultLeaderboardPegSize,
                       /*x => !string.IsNullOrEmpty(x.WinningBets),*/ x => x.Rank, true).ToList();
                }

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
