

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

    public class MemberSearchController : BaseController
    {
        private readonly IUserRankDataRepository _userRankDataRepository;
        private readonly int _defaultMemberPageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="RankingController"/> class.
        /// </summary>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public MemberSearchController(IUserRankDataRepository userRankDataRepository)
        {
            this._userRankDataRepository = userRankDataRepository;
            this._defaultMemberPageSize = CommonUtility.GetConfigData<int>("DefaultMamberPageSize");
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
                List<UserRankViewModel> user = this._userRankDataRepository
                    .GetList(1, this._defaultMemberPageSize, null, x => x.Rank, true).ToList();

                if (user != null)
                {
                    ViewBag.TotalRecord = this._userRankDataRepository.GetCount();
                    ViewBag.PageSize = this._defaultMemberPageSize;
                    ViewBag.PagingUrl = Url.Content("~/MemberSearch/GetMemberByPage");
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
        /// Gets the member by page.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult GetMemberByPage(int pageIndex, string filter = "", string orderBy = "")
        {
            try
            {
                List<UserRankViewModel> user = new List<UserRankViewModel>();
                Expression<Func<UserRankViewModel, bool>> exp = null;
                int recordsToPick = pageIndex;

                if (!string.IsNullOrEmpty(filter))
                    exp = CommonUtility.GetLamdaExpressionFromFilter<UserRankViewModel>(filter);

                if (orderBy == "MemberSince")
                    user = this._userRankDataRepository
                    .GetListCompiled(recordsToPick, this._defaultMemberPageSize, exp, x => x.MemberSince, false).ToList();
                else if (orderBy == "Wins")
                    user = this._userRankDataRepository
                    .GetListCompiled(recordsToPick, this._defaultMemberPageSize, exp, x => x.Wins, false).ToList();
                else if (orderBy == "Losses")
                    user = this._userRankDataRepository
                    .GetListCompiled(recordsToPick, this._defaultMemberPageSize, exp, x => x.Losses, false).ToList();
                else if (orderBy == "BetWins")
                    user = this._userRankDataRepository
                    .GetListCompiled(recordsToPick, this._defaultMemberPageSize, exp, x => x.BetWins, false).ToList();
                else if (orderBy == "Profit")
                    user = this._userRankDataRepository
                    .GetListCompiled(recordsToPick, this._defaultMemberPageSize, exp, x => x.Profit, false).ToList();
                else if (orderBy == "BookMakerName")
                    user = this._userRankDataRepository
                    .GetListCompiled(recordsToPick, this._defaultMemberPageSize, exp, x => x.BookMakerName, true).ToList();
                else
                    user = this._userRankDataRepository
                    .GetListCompiled(recordsToPick, this._defaultMemberPageSize, exp, x => x.Rank, true).ToList();

                if (user != null)
                {
                    ViewBag.TotalRecord = this._userRankDataRepository.GetCountCompiled(exp);
                    ViewBag.PageSize = this._defaultMemberPageSize;
                    ViewBag.LastElement = 2;
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
