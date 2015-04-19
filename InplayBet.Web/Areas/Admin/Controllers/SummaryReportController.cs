

namespace InplayBet.Web.Areas.Admin.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class SummaryReportController : BaseController
    {
        private readonly IUserRankDataRepository _userRankDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryReportController" /> class.
        /// </summary>
        /// <param name="userRankDataRepository">The user rank data repository.</param>
        public SummaryReportController(IUserRankDataRepository userRankDataRepository)
        {
            this._userRankDataRepository = userRankDataRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Index()
        {
            ViewBag.SchemaData = UserSummaryModelSchema();
            return View();
        }

        /// <summary>
        /// Gets the user summary.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult GetUserSummary()
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
                            x.MemberSince,
                            x.UserName,
                            x.EmailId,
                            x.Address,
                            Sex = (x.Sex == "M") ? SexType.Male.ToString() : SexType.Female.ToString(),
                            x.BookMakerName,
                            x.Wins,
                            x.Losses,
                            x.TotalBets
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
        /// Users the summary model schema.
        /// </summary>
        /// <returns></returns>
        private string UserSummaryModelSchema()
        {
            string strSchema = string.Empty;
            GridModelSchema gridModelSchema = new GridModelSchema();
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
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.MemberSince),
                    formatoptions = new formatoptions { newformat = "d-m-Y" },
                    formatter = Formatter.date.ToString(),
                    align = Align.center.ToString(),
                    width = 30,
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
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.EmailId),
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
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.Address),
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
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.Sex),
                    editable = true,
                    width = 20,
                    align = Align.center.ToString(),
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
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.BookMakerName),
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
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.Wins),
                    align = Align.right.ToString(),
                    width = 20,
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
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.Losses),
                    align = Align.right.ToString(),
                    width = 20,
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
                    name = CommonUtility.GetDisplayName((UserRankViewModel x) => x.TotalBets),
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
                    name = "Actions",
                    index = "act",
                    width = 15,
                    sortable = false,
                    search = false
                });

                gridModelSchema = new GridModelSchema(columnModel);
                var labels = new List<string>();
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.UserKey));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.UserId, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.MemberSince, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.UserName, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.EmailId, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.Address, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.Sex, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.BookMakerName, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.Wins, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.Losses, true));
                labels.Add(CommonUtility.GetDisplayName((UserRankViewModel x) => x.TotalBets, true));
                labels.Add("Delete");
                gridModelSchema.colNames = labels.ToArray();

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
            return strSchema;
        }
    }
}
