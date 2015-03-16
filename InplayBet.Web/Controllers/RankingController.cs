
namespace InplayBet.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Utilities;
    using InplayBet.Web.Utilities.Expression;

    public class RankingController : BaseController
    {
        private readonly IUserRankDataRepository _userRankDataRepository;
        private readonly int _defaultRankPageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="RankingController"/> class.
        /// </summary>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public RankingController(IUserRankDataRepository userRankDataRepository)
        {
            this._userRankDataRepository = userRankDataRepository;
            this._defaultRankPageSize = CommonUtility.GetConfigData<int>("DefaultRankPageSize");
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
                Expression<Func<UserRankViewModel, bool>> exp = (x) => !string.IsNullOrEmpty(x.WinningBets);

                List<UserRankViewModel> user = this._userRankDataRepository
                    .GetList(1, this._defaultRankPageSize, exp, x => x.Rank, true).ToList();

                if (user != null)
                {
                    ViewBag.TotalRecord = this._userRankDataRepository.GetCount(exp);
                    ViewBag.PageSize = this._defaultRankPageSize;
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
        /// Gets the rank by page.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult GetRankByPage(int pageIndex, string filter = null)
        {
            try
            {
                Expression<Func<UserRankViewModel, bool>> exp = (x) => !string.IsNullOrEmpty(x.WinningBets);
                int recordsToPick = (this._defaultRankPageSize * pageIndex);

                if (!string.IsNullOrEmpty(filter))
                {
                    Expression<Func<UserRankViewModel, bool>> filterExp
                        = CommonUtility.GetLamdaExpressionFromFilter<UserRankViewModel>(filter);
                    exp = exp.And(filterExp);
                }

                List<UserRankViewModel> user = this._userRankDataRepository
                    .GetList(recordsToPick, this._defaultRankPageSize, exp, x => x.Rank, true).ToList();

                if (user != null)
                {
                    ViewBag.TotalRecord = this._userRankDataRepository.GetCount(exp);
                    return PartialView("_UserRank", user);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(pageIndex, filter);
            }
            return null;
        }
    }
}
