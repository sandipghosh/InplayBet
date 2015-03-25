

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

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryReportController" /> class.
        /// </summary>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public CheatReportController(IUserRankDataRepository userRankDataRepository)
        {
            this._userRankDataRepository = userRankDataRepository;
        }

        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Index()
        {
            ViewBag.SchemaData = GetReportedUsersModelSchema();
            ViewBag.SchemaSubData = GetReportingUsersModelSchema();
            return View();
        }

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
                        var searchCriteria = CommonUtility.GetLamdaExpressionFromFilter<UserRankViewModel>(criteria);
                        totalRecords = this._userRankDataRepository.GetCountCompiled(searchCriteria);
                        users = this._userRankDataRepository.GetListCompiled(requestSearchData.page,
                            requestSearchData.rows, searchCriteria, x => x.UserId, false).ToList();
                    }
                    else
                    {
                        totalRecords = this._userRankDataRepository.GetCount();
                        users = this._userRankDataRepository.GetList(requestSearchData.page, requestSearchData.rows,
                            null, x => x.UserId, false).ToList();
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

        private string GetReportingUsersModelSchema()
        {
            GridModelSchema gridModelSchema = null;
            try
            {
                List<colModel> columnModel = new List<colModel>();
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
