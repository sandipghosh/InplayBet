

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
        public ActionResult GetMemberByPage(int pageIndex, string filter = null)
        {
            try
            {
                Expression<Func<UserRankViewModel, bool>> exp = null;
                int recordsToPick = (this._defaultMemberPageSize * pageIndex);

                if (!string.IsNullOrEmpty(filter))
                    exp = CommonUtility.GetLamdaExpressionFromFilter<UserRankViewModel>(filter);

                List<UserRankViewModel> user = this._userRankDataRepository
                    .GetList(recordsToPick, this._defaultMemberPageSize, null, x => x.Rank, true).ToList();

                if (user != null)
                {
                    ViewBag.TotalRecord = this._userRankDataRepository.GetCount(exp);
                    return PartialView(user);
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
