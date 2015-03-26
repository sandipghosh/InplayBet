

namespace InplayBet.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using Newtonsoft.Json;
    using InplayBet.Web.Controllers.Base;

    public class CheatReportController : BaseController
    {
        private readonly IUserRankDataRepository _userRankDataRepository;
        private readonly IReportDataRepository _reportDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryReportController" /> class.
        /// </summary>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public CheatReportController(IUserRankDataRepository userRankDataRepository,
            IReportDataRepository reportDataRepository)
        {
            this._userRankDataRepository = userRankDataRepository;
            this._reportDataRepository = reportDataRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Index()
        {
            ViewBag.SchemaData = GetReportedUsersModelSchema();
            ViewBag.SchemaSubData = GetReportingUsersModelSchema();
            return View();
        }

        /// <summary>
        /// Gets the reported users.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult GetReportedUsers()
        {
            try
            {
                GridSearchDataModel requestSearchData = new GridSearchDataModel();
                requestSearchData.SetPropertiesFromContext<GridSearchDataModel>(System.Web.HttpContext.Current);

                if (requestSearchData != null)
                {
                    List<UserRankViewModel> users = new List<UserRankViewModel>();
                    int totalRecords = 0;

                    if (requestSearchData._search)
                    {
                        string criteria = requestSearchData.filters.ToString();
                        var searchCriteria = CommonUtility.GetLamdaExpressionFromFilter<UserRankViewModel>(string.Format("TotalCheatReported > 0 and {0}", criteria));

                        totalRecords = this._userRankDataRepository.GetCountCompiled(searchCriteria);
                        users = this._userRankDataRepository.GetListCompiled(requestSearchData.page,
                            requestSearchData.rows, searchCriteria, x => x.UserId, false).ToList();
                    }
                    else
                    {
                        totalRecords = this._userRankDataRepository.GetCount();
                        users = this._userRankDataRepository.GetList(requestSearchData.page, requestSearchData.rows,
                            x => x.TotalCheatReported > 0, x => x.UserId, false).ToList();
                    }

                    return new JsonActionResult(new GridDataModel()
                    {
                        currpage = requestSearchData.page,
                        totalpages = (int)Math.Ceiling((float)totalRecords / (float)requestSearchData.rows),
                        totalrecords = totalRecords,
                        invdata = users.Select(x => new
                        {
                            x.UserKey,
                            x.UserId,
                            x.UserName,
                            x.TotalChallenges,
                            x.TotalCheatReported
                        })
                    });
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        /// <summary>
        /// Gets the reporting users.
        /// </summary>
        /// <param name="UserKey">The user key.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult GetReportingUsers(int UserKey)
        {
            try
            {
                var cheatReports = this._reportDataRepository.GetCheatCountByUser(UserKey).ToList();
                return new JsonActionResult(cheatReports);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return RedirectToAction("Index", "Error", new { area = "" });
        }

        /// <summary>
        /// Gets the reported users model schema.
        /// </summary>
        /// <returns></returns>
        private string GetReportedUsersModelSchema()
        {
            GridModelSchema gridModelSchema = null;
            try
            {
                List<colModel> columnModel = new List<colModel>();
                columnModel.Add(new colModel()
                {
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.UserKey),
                    editable = true,
                    hidden = true,
                    key = true,
                    edittype = Edittype.custom.ToString(),
                });
                columnModel.Add(new colModel()
                {
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.UserId),
                    caption = CommonUtility.GetDisplayName((UserRankViewModel x) => x.UserId, true),
                    align = Align.left.ToString(),
                    width = 30,
                    searchoptions = new SearchOptions
                    {
                        sopt = new string[] 
                        {
                            SearchOperator.eq.ToString(), 
                            SearchOperator.ne.ToString(),
                            SearchOperator.bw.ToString(),
                            SearchOperator.bn.ToString(),
                            SearchOperator.cn.ToString(),
                            SearchOperator.nc.ToString(),
                            SearchOperator.ew.ToString(),
                            SearchOperator.en.ToString(),
                        }
                    }
                });
                columnModel.Add(new colModel()
                {
                    caption = CommonUtility.GetDisplayName((UserRankViewModel x) => x.UserName, true),
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.UserName),
                    align = Align.left.ToString(),
                    width = 50,
                    searchoptions = new SearchOptions
                    {
                        sopt = new string[] 
                        {
                            SearchOperator.eq.ToString(), 
                            SearchOperator.ne.ToString(),
                            SearchOperator.bw.ToString(),
                            SearchOperator.bn.ToString(),
                            SearchOperator.cn.ToString(),
                            SearchOperator.nc.ToString(),
                            SearchOperator.ew.ToString(),
                            SearchOperator.en.ToString(),
                        }
                    }
                });
                columnModel.Add(new colModel()
                {
                    caption = CommonUtility.GetDisplayName((UserRankViewModel x) => x.TotalChallenges, true),
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.TotalChallenges),
                    align = Align.right.ToString(),
                    width = 30,
                    formatter = Formatter.integer.ToString(),
                    searchoptions = new SearchOptions
                    {
                        sopt = new string[] 
                        {
                            SearchOperator.eq.ToString(), 
                            SearchOperator.ne.ToString(),
                            SearchOperator.le.ToString(),
                            SearchOperator.lt.ToString(),
                            SearchOperator.gt.ToString(),
                            SearchOperator.ge.ToString()
                        }
                    }
                });
                columnModel.Add(new colModel()
                {
                    caption = CommonUtility.GetDisplayName((UserRankViewModel x) => x.TotalCheatReported, true),
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.TotalCheatReported),
                    align = Align.right.ToString(),
                    width = 30,
                    formatter = Formatter.integer.ToString(),
                    searchoptions = new SearchOptions
                    {
                        sopt = new string[] 
                        {
                            SearchOperator.eq.ToString(), 
                            SearchOperator.ne.ToString(),
                            SearchOperator.le.ToString(),
                            SearchOperator.lt.ToString(),
                            SearchOperator.gt.ToString(),
                            SearchOperator.ge.ToString()
                        }
                    }
                });

                gridModelSchema = new GridModelSchema(columnModel);
                return JsonConvert.SerializeObject(gridModelSchema, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                }).ToBase64Encode();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the reporting users model schema.
        /// </summary>
        /// <returns></returns>
        private string GetReportingUsersModelSchema()
        {
            GridModelSchema gridModelSchema = null;
            try
            {
                List<colModel> columnModel = new List<colModel>();
                columnModel.Add(new colModel()
                {
                    name = CommonUtility.GetDisplayName((ReportCountViewModel x) => x.UserId),
                    caption = CommonUtility.GetDisplayName((ReportCountViewModel x) => x.UserId, true),
                    align = Align.left.ToString(),
                    width = 30,
                    searchoptions = new SearchOptions
                    {
                        sopt = new string[] 
                        {
                            SearchOperator.eq.ToString(), 
                            SearchOperator.ne.ToString(),
                            SearchOperator.bw.ToString(),
                            SearchOperator.bn.ToString(),
                            SearchOperator.cn.ToString(),
                            SearchOperator.nc.ToString(),
                            SearchOperator.ew.ToString(),
                            SearchOperator.en.ToString(),
                        }
                    }
                });
                columnModel.Add(new colModel()
                {
                    name = CommonUtility.GetDisplayName((ReportCountViewModel x) => x.UserName),
                    caption = CommonUtility.GetDisplayName((ReportCountViewModel x) => x.UserName, true),
                    align = Align.left.ToString(),
                    width = 30,
                    searchoptions = new SearchOptions
                    {
                        sopt = new string[] 
                        {
                            SearchOperator.eq.ToString(), 
                            SearchOperator.ne.ToString(),
                            SearchOperator.bw.ToString(),
                            SearchOperator.bn.ToString(),
                            SearchOperator.cn.ToString(),
                            SearchOperator.nc.ToString(),
                            SearchOperator.ew.ToString(),
                            SearchOperator.en.ToString(),
                        }
                    }
                });
                columnModel.Add(new colModel()
                {
                    name = CommonUtility.GetDisplayName((ReportCountViewModel x) => x.ReportCount),
                    caption = CommonUtility.GetDisplayName((ReportCountViewModel x) => x.ReportCount, true),
                    align = Align.left.ToString(),
                    width = 30,
                    searchoptions = new SearchOptions
                    {
                        sopt = new string[] 
                        {
                            SearchOperator.eq.ToString(), 
                            SearchOperator.ne.ToString(),
                            SearchOperator.bw.ToString(),
                            SearchOperator.bn.ToString(),
                            SearchOperator.cn.ToString(),
                            SearchOperator.nc.ToString(),
                            SearchOperator.ew.ToString(),
                            SearchOperator.en.ToString(),
                        }
                    }
                });

                gridModelSchema = new GridModelSchema(columnModel);
                return JsonConvert.SerializeObject(gridModelSchema, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                }).ToBase64Encode();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return string.Empty;
        }
    }
}
