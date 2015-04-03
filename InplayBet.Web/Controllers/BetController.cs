

namespace InplayBet.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;
    using System.Web.Mvc;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using MoreLinq;
    using System.Web;

    public class BetController : Controller
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IBetDataRepository _betDataRepository;
        private readonly ITeamDataRepository _teamDataRepository;
        private readonly ILegueDataRepository _legueDataRepository;
        private readonly IChallengeDataRepository _challengeDataRepository;
        private readonly IReportDataRepository _reportDataRepository;
        private readonly IUserRankDataRepository _userRankDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetController" /> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="betDataRepository">The bet data repository.</param>
        /// <param name="teamDataRepository">The team data repository.</param>
        /// <param name="legueDataRepository">The legue data repository.</param>
        /// <param name="challengeDataRepository">The challenge data repository.</param>
        public BetController(IUserDataRepository userDataRepository,
            IBetDataRepository betDataRepository,
            ITeamDataRepository teamDataRepository,
            ILegueDataRepository legueDataRepository,
            IChallengeDataRepository challengeDataRepository,
            IReportDataRepository reportDataRepository,
            IUserRankDataRepository userRankDataRepository)
        {
            this._userDataRepository = userDataRepository;
            this._betDataRepository = betDataRepository;
            this._teamDataRepository = teamDataRepository;
            this._legueDataRepository = legueDataRepository;
            this._challengeDataRepository = challengeDataRepository;
            this._reportDataRepository = reportDataRepository;
            this._userRankDataRepository = userRankDataRepository;
        }

        /// <summary>
        /// Gets the challenges.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetChallengesByUser(int userKey)
        {
            try
            {
                var challenges = GetChallenges(userKey, true).ToList();
                if (!challenges.IsEmptyCollection())
                {
                    ChallengeModel lastCompletedChallenge = challenges.MaxBy(x => x.ChallengeId);
                    if (!string.IsNullOrEmpty(lastCompletedChallenge.ChallengeStatus) &&
                        (userKey == SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey)))
                    {
                        challenges.Insert(0, new ChallengeModel
                        {
                            ChallengeNumber = lastCompletedChallenge.ChallengeNumber + 1,
                            StatusId = (int)StatusCode.Active,
                            ChallengeStatus = string.Empty,
                            WiningPrice = 0,
                            Bets = new List<BetModel>(),
                            UserKey = userKey,
                            CreatedBy = userKey,
                            CreatedOn = DateTime.Now
                        });
                    }
                }

                ViewBag.CultureCode = GetUserCaltureCode(userKey);
                ViewBag.UserKey = userKey;
                return View(challenges);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return null;
        }

        /// <summary>
        /// Gets the bets by challenge.
        /// </summary>
        /// <param name="challengeId">The challenge identifier.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult GetBetsByChallenge(int challengeId)
        {
            try
            {
                ChallengeModel challenge = this._challengeDataRepository.Get(challengeId);
                if (challenge != null)
                {
                    var bets = challenge.Bets.OrderByDescending(x => x.BetId).ToList();
                    ViewBag.CurrentChallenge = challenge;
                    ViewBag.UserKey = challenge.UserKey;
                    ViewBag.CultureCode = GetUserCaltureCode(challenge.UserKey);
                    return View(bets);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(challengeId);
            }
            return null;
        }

        /// <summary>
        /// Shows the new bet window.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowNewBetWindow(int userKey)
        {
            try
            {
                BetModel bet = new BetModel()
                {
                    Challenge = GetRecentChallenge(userKey),
                    StatusId = (int)StatusCode.Active,
                    CreatedBy = userKey,
                    CreatedOn = DateTime.Now,
                    BetStatus = string.Empty,

                    TeamAName = "",
                    TeamBName = "",
                    LegueName = ""
                };
                bet.ChallengeId = bet.Challenge.ChallengeId;
                bet.BetNumber = GetBetNumber(bet.ChallengeId);
                bet.BetPlaced = GetBetPlacedAmount(bet.ChallengeId);

                ViewBag.UserKey = userKey;
                ViewBag.CultureCode = GetUserCaltureCode(userKey);
                ViewBag.BetDisplayMode = (userKey == SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey)) ?
                    BetDisplayType.Insert.ToString() : BetDisplayType.Read.ToString();
                return View(bet);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return null;
        }

        /// <summary>
        /// Shows the new bet window overwride.
        /// </summary>
        /// <param name="challenge">The challenge.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowNewBetWindowOverwride(ChallengeModel challenge)
        {
            try
            {
                challenge.CreatedOn = (challenge.CreatedOn.Year == 1)
                    ? DateTime.Now : challenge.CreatedOn;

                BetModel bet = new BetModel()
                {
                    Challenge = challenge,
                    StatusId = (int)StatusCode.Active,
                    CreatedBy = challenge.UserKey,
                    CreatedOn = challenge.CreatedOn,
                    BetStatus = string.Empty,

                    TeamAName = "",
                    TeamBName = "",
                    LegueName = ""
                };
                bet.ChallengeId = bet.Challenge.ChallengeId;
                bet.BetNumber = (bet.ChallengeId == 0) ? 1 : GetBetNumber(bet.ChallengeId);
                bet.BetPlaced = (bet.ChallengeId == 0) ?
                    CommonUtility.GetConfigData<decimal>("StartingBetAmount") : GetBetPlacedAmount(bet.ChallengeId);

                ViewBag.UserKey = challenge.UserKey;
                ViewBag.CultureCode = GetUserCaltureCode(challenge.UserKey);
                ViewBag.BetDisplayMode = (challenge.UserKey == SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey)) ?
                    BetDisplayType.Insert.ToString() : BetDisplayType.Read.ToString();
                return View("ShowNewBetWindow", bet);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(challenge);
            }
            return null;
        }

        /// <summary>
        /// Inserts the new bet.
        /// </summary>
        /// <param name="bet">The bet.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult InsertNewBet(BetModel bet)
        {
            try
            {
                int userKey = bet.Challenge.UserKey;
                Action<BetModel> processTeamAndLegue = (b) =>
                {
                    var context = ControllerContext.HttpContext.Request;
                    b.TeamAId = InsertTeam(b, "a", context);
                    b.TeamBId = InsertTeam(b, "b", context);
                    b.LegueId = InsertLegue(b, context);
                };

                if (bet.ChallengeId == 0)
                {
                    TransactionOptions options = new TransactionOptions()
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = new TimeSpan(0, 5, 0)
                    };
                    using (TransactionScope scope = new TransactionScope
                        (TransactionScopeOption.RequiresNew, options))
                    {
                        ChallengeModel challenge = bet.Challenge;
                        challenge.ChallengeStatus = challenge.ChallengeStatus.AsString();
                        challenge.CreatedOn = DateTime.Now;
                        this._challengeDataRepository.Insert(challenge);

                        bet.ChallengeId = challenge.ChallengeId;
                        bet.BetStatus = bet.BetStatus.AsString();
                        bet.CreatedOn = DateTime.Now;
                        bet.Challenge = null;

                        processTeamAndLegue(bet);

                        this._betDataRepository.Insert(bet);
                        if (bet.BetId == 0) scope.Dispose(); else scope.Complete();
                    }
                }
                else
                {
                    bet.BetStatus = bet.BetStatus.AsString();
                    bet.Challenge = null;
                    processTeamAndLegue(bet);
                    this._betDataRepository.Insert(bet);
                }

                if (bet.BetId > 0)
                {
                    try
                    {
                        BetSubmitNotification(userKey, bet);
                    }
                    catch (Exception) { }
                }
                return new JsonActionResult(bet);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bet);
            }
            return null;
        }

        /// <summary>
        /// Updates the bet status.
        /// </summary>
        /// <param name="bet">The bet.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult UpdateBetStatus(BetModel bet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Func<BetModel, ChallengeModel> getCurrentChallenge = (b) =>
                    {
                        if ((b.WiningTotal >= CommonUtility.GetConfigData<decimal>("WiningBetAmount")
                            && b.BetStatus == StatusCode.Won.ToString()) || b.BetStatus == StatusCode.Lost.ToString())
                        {
                            int chellangeId = b.ChallengeId;
                            return this._challengeDataRepository.Get(chellangeId);
                        }
                        return null;
                    };
                    ChallengeModel challenge = getCurrentChallenge(bet);

                    TransactionOptions options = new TransactionOptions()
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = new TimeSpan(0, 1, 0)
                    };
                    using (TransactionScope scope = new TransactionScope
                        (TransactionScopeOption.RequiresNew, options))
                    {
                        bet.UpdatedOn = DateTime.Now;
                        bet.UpdatedBy = bet.CreatedBy;
                        this._betDataRepository.Update(bet);

                        if (challenge != null)
                        {
                            challenge.UpdatedBy = challenge.CreatedBy;
                            challenge.UpdatedOn = DateTime.Now;
                            challenge.ChallengeStatus = bet.BetStatus;
                            challenge.WiningPrice = (bet.BetStatus == StatusCode.Lost.ToString()) ?
                                ((CommonUtility.GetConfigData<decimal>("StartingBetAmount")) * (-1)) : bet.WiningTotal;

                            this._challengeDataRepository.Update(challenge);
                        }

                        scope.Complete();
                        return new JsonActionResult(bet);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bet);
            }
            return null;
        }

        /// <summary>
        /// Shows the challenge status message.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult ShowChallengeStatusMessage(string status)
        {
            ViewBag.Status = status;
            return View();
        }

        /// <summary>
        /// Shows the report window.
        /// </summary>
        /// <param name="reportToUserKey">The report to user key.</param>
        /// <param name="challengeId">The challenge identifier.</param>
        /// <param name="challengeStatus">The challenge status.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult ShowReportWindow(int reportToUserKey, int challengeId, string challengeStatus)
        {
            try
            {
                ReportModel report = new ReportModel()
                {
                    ReportedUserId = reportToUserKey,
                    ReportedChallengeId = challengeId,
                    CreatedBy = SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey),
                    CreatedOn = DateTime.Now,
                    ReportedBy = SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey),
                    StatusId = (int)StatusCode.Active,
                    ReportStatus = (challengeStatus == StatusCode.Won.ToString()) ?
                        StatusCode.Lost.ToString() : StatusCode.Won.ToString()
                };

                List<SelectListItem> reportStatusList = new List<SelectListItem>()
                    {new SelectListItem{Text=StatusCode.Won.ToString(), Value=StatusCode.Won.ToString()},
                    {new SelectListItem{Text=StatusCode.Lost.ToString(), Value=StatusCode.Lost.ToString()}}};

                ViewBag.ReportStatus = reportStatusList;
                return View(report);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(reportToUserKey, challengeId, challengeStatus);
            }
            return null;
        }

        /// <summary>
        /// Submits the report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult SubmitReport(ReportModel report)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this._reportDataRepository.Insert(report);
                }
                return new JsonActionResult(report);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(report);
            }
            return null;
        }


        #region Private Members
        /// <summary>
        /// Gets the challenge.
        /// </summary>
        /// <returns></returns>
        private ChallengeModel GetRecentChallenge(int userKey)
        {
            ChallengeModel currentChallenge = new ChallengeModel();
            try
            {
                Func<List<ChallengeModel>, int, ChallengeModel> getLastChellenge =
                    (c, id) => c.FirstOrDefault(x => x.ChallengeId.Equals(id));

                Func<int, ChallengeModel> createChallengeInstance = (num) =>
                    new ChallengeModel
                    {
                        ChallengeNumber = num,
                        StatusId = (int)StatusCode.Active,
                        WiningPrice = 0,
                        UserKey = userKey,
                        CreatedBy = userKey,
                        CreatedOn = DateTime.Now,
                        ChallengeStatus = string.Empty,
                    };

                var challenges = GetChallenges(userKey);
                if (challenges.IsEmptyCollection())
                {
                    return createChallengeInstance(1);
                }
                else
                {
                    BetModel bet = challenges.MaxBy(x => x.ChallengeId).Bets
                        .Where(x => x.StatusId.Equals((int)StatusCode.Active))
                        .MaxBy(x => x.BetId);

                    if (bet.BetStatus.AsString() == StatusCode.Lost.ToString() ||
                        (bet.BetStatus.AsString() == StatusCode.Won.ToString()
                        && bet.WiningTotal >= CommonUtility.GetConfigData<decimal>("WiningBetAmount")))
                    {
                        ChallengeModel challenge = getLastChellenge(challenges, bet.ChallengeId);
                        return createChallengeInstance(challenge.ChallengeNumber + 1);
                    }
                    else
                    { return getLastChellenge(challenges, bet.ChallengeId); }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return currentChallenge;
        }

        /// <summary>
        /// Gets the bet number.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <param name="challengeId">The challenge identifier.</param>
        /// <returns></returns>
        private int GetBetNumber(int challengeId)
        {
            try
            {
                BetModel lastBet = null;
                var bets = GetBets(challengeId, true);

                if (!bets.IsEmptyCollection())
                    lastBet = bets.MaxBy(x => x.BetId);

                if (lastBet == null)
                    return 1;
                else
                    return lastBet.BetNumber + 1;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(challengeId);
            }
            return 1;
        }

        /// <summary>
        /// Gets the bet placed amount.
        /// </summary>
        /// <param name="challengeId">The challenge identifier.</param>
        /// <returns></returns>
        private decimal GetBetPlacedAmount(int challengeId)
        {
            try
            {
                BetModel lastBet = null;
                var bets = GetBets(challengeId);

                if (!bets.IsEmptyCollection())
                    lastBet = bets.MaxBy(x => x.BetId);

                if (lastBet == null)
                    return CommonUtility.GetConfigData<decimal>("StartingBetAmount");
                else
                    return lastBet.WiningTotal;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(challengeId);
            }
            return CommonUtility.GetConfigData<decimal>("StartingBetAmount");
        }

        /// <summary>
        /// Gets the bets.
        /// </summary>
        /// <param name="challengeId">The challenge identifier.</param>
        /// <returns></returns>
        private List<BetModel> GetBets(int challengeId, bool useSorting = false)
        {
            try
            {
                if (useSorting)
                    return this._betDataRepository.GetList(x => x.ChallengeId.Equals(challengeId)
                    && x.StatusId.Equals((int)StatusCode.Active), x => x.BetId, false).ToList();
                else
                    return this._betDataRepository.GetList(x => x.ChallengeId.Equals(challengeId)
                        && x.StatusId.Equals((int)StatusCode.Active)).ToList();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(challengeId, useSorting);
            }
            return null;
        }

        /// <summary>
        /// Gets the challenges.
        /// </summary>
        /// <param name="userKey">The user identifier.</param>
        /// <returns></returns>
        private List<ChallengeModel> GetChallenges(int userKey, bool useSorting = false)
        {
            try
            {
                if (useSorting)
                    return this._challengeDataRepository.GetList(x => x.UserKey.Equals(userKey)
                    && x.StatusId.Equals((int)StatusCode.Active), x => x.ChallengeId, false).ToList();
                else
                    return this._challengeDataRepository.GetList(x => x.UserKey.Equals(userKey)
                        && x.StatusId.Equals((int)StatusCode.Active)).ToList();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey, useSorting);
            }
            return null;
        }

        /// <summary>
        /// Gets the user calture code.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns></returns>
        private string GetUserCaltureCode(int userKey)
        {
            try
            {
                UserModel user = this._userDataRepository.Get(userKey);
                if (user != null)
                {
                    return user.Currency.CultureCode;
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return string.Empty;
        }

        /// <summary>
        /// Bets the submit notification.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <param name="currentBet">The current bet.</param>
        private void BetSubmitNotification(int userKey, BetModel currentBet)
        {
            try
            {
                BetNotificationViewModel betViewModel = new BetNotificationViewModel
                {
                    Bet = currentBet,
                    User = this._userRankDataRepository
                        .GetList(x => x.UserKey.Equals(userKey)).FirstOrDefault()
                };
                int consicutiveWonBets = this._betDataRepository.GetConsicutiveBetWins(userKey);
                string mailContent = CommonUtility.RenderViewToString("_BetSubmissionMailNotification", betViewModel,
                    this, new Dictionary<string, object>() { { "ConsicutiveWonBets", consicutiveWonBets } });

                SharedFunctionality shared = new SharedFunctionality();
                var followerUsers = shared.GetFollowerUsers(userKey);
                if (!followerUsers.IsEmptyCollection())
                {
                    shared.MassMailing(followerUsers.Select(x => x.EmailId).ToList(), mailContent, 
                        "Inplay Bet Submit Notification");
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey, currentBet);
            }
        }

        /// <summary>
        /// Inserts the team.
        /// </summary>
        /// <param name="bet">The bet.</param>
        /// <param name="targetTeam">The target team.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private int InsertTeam(BetModel bet, string targetTeam, HttpRequestBase context)
        {
            try
            {
                Func<BetModel, int, string, int> insertionPrecess = (b, betTeamId, teamName) =>
                {
                    if (betTeamId > 0)
                        return betTeamId;
                    else
                    {
                        TeamModel team = new TeamModel();
                        if (this._teamDataRepository.Exists(x => x.TeamName.Equals(teamName)
                            && x.StatusId.Equals((int)StatusCode.Active)))
                        {
                            team = this._teamDataRepository.GetList(x => x.TeamName.Equals(teamName)
                                && x.StatusId.Equals((int)StatusCode.Active)).FirstOrDefault();
                            return team.TeamId;
                        }
                        else if (!string.IsNullOrEmpty(teamName))
                        {
                            team.TeamName = teamName;
                            team.StatusId = (int)StatusCode.Active;
                            team.CreatedOn = DateTime.Now;
                            team.CreatedBy = b.CreatedBy;
                            this._teamDataRepository.Insert(team);
                            return (team.TeamId > 0) ? team.TeamId : 0;
                        }
                    }
                    return 0;
                };

                if (targetTeam == "a")
                {
                    return insertionPrecess(bet, bet.TeamAId, context.Form["TeamAName"].AsString());
                }
                else if (targetTeam == "b")
                {
                    return insertionPrecess(bet, bet.TeamBId, context.Form["TeamBName"].AsString());
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bet, context);
            }
            return 0;
        }

        /// <summary>
        /// Inserts the legue.
        /// </summary>
        /// <param name="bet">The bet.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private int InsertLegue(BetModel bet, HttpRequestBase context)
        {
            try
            {
                if (bet.LegueId > 0)
                    return bet.LegueId;
                else
                {
                    LegueModel legue = new LegueModel();
                    string legueName = context.Form["LegueName"].AsString();
                    if (this._legueDataRepository.Exists(x => x.LegueName.Equals(legueName)
                        && x.StatusId.Equals((int)StatusCode.Active)))
                    {
                        legue = this._legueDataRepository.GetList(x => x.LegueName.Equals(legueName)
                            && x.StatusId.Equals((int)StatusCode.Active)).FirstOrDefault();
                        return legue.LegueId;
                    }
                    else if (!string.IsNullOrEmpty(legueName))
                    {
                        legue.LegueName = legueName;
                        legue.StatusId = (int)StatusCode.Active;
                        legue.CreatedOn = DateTime.Now;
                        legue.CreatedBy = bet.CreatedBy;
                        this._legueDataRepository.Insert(legue);
                        return (legue.LegueId > 0) ? legue.LegueId : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bet, context);
            }
            return 0;
        }
        #endregion
    }
}
