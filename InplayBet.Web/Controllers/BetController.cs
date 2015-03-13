﻿

namespace InplayBet.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;
    using System.Web.Mvc;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using MoreLinq;

    public class BetController : Controller
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IBetDataRepository _betDataRepository;
        private readonly ITeamDataRepository _teamDataRepository;
        private readonly ILegueDataRepository _legueDataRepository;
        private readonly IChallengeDataRepository _challengeDataRepository;

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
            IChallengeDataRepository challengeDataRepository)
        {
            this._userDataRepository = userDataRepository;
            this._betDataRepository = betDataRepository;
            this._teamDataRepository = teamDataRepository;
            this._legueDataRepository = legueDataRepository;
            this._challengeDataRepository = challengeDataRepository;
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
                    if (!string.IsNullOrEmpty(lastCompletedChallenge.ChallengeStatus))
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
                    BetStatus = string.Empty
                };
                bet.ChallengeId = bet.Challenge.ChallengeId;
                bet.BetNumber = GetBetNumber(bet.ChallengeId);
                bet.BetPlaced = GetBetPlacedAmount(bet.ChallengeId);

                ViewBag.UserKey = userKey;
                ViewBag.BetDisplayMode = (userKey == SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey)) ?
                    BetDisplayType.Insert.ToString() : BetDisplayType.Read.ToString();
                return View(bet);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
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
                BetModel bet = new BetModel()
                {
                    Challenge = challenge,
                    StatusId = (int)StatusCode.Active,
                    CreatedBy = challenge.UserKey,
                    CreatedOn = challenge.CreatedOn,
                    BetStatus = string.Empty
                };
                bet.ChallengeId = bet.Challenge.ChallengeId;
                bet.BetNumber = (bet.ChallengeId == 0) ? 1 : GetBetNumber(bet.ChallengeId);
                bet.BetPlaced = (bet.ChallengeId == 0) ?
                    CommonUtility.GetConfigData<decimal>("StartingBetAmount") : GetBetPlacedAmount(bet.ChallengeId);

                ViewBag.UserKey = challenge.UserKey;
                ViewBag.BetDisplayMode = (challenge.UserKey == SessionVeriables.GetSessionData<int>(SessionVeriables.UserKey)) ?
                    BetDisplayType.Insert.ToString() : BetDisplayType.Read.ToString();
                return View("ShowNewBetWindow", bet);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
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
                if (ModelState.IsValid)
                {
                    if (bet.ChallengeId == 0)
                    {
                        TransactionOptions options = new TransactionOptions()
                        {
                            IsolationLevel = IsolationLevel.ReadCommitted,
                            Timeout = new TimeSpan(0, 1, 0)
                        };
                        using (TransactionScope scope = new TransactionScope
                            (TransactionScopeOption.RequiresNew, options))
                        {
                            ChallengeModel challenge = bet.Challenge;
                            challenge.ChallengeStatus = challenge.ChallengeStatus.AsString();
                            this._challengeDataRepository.Insert(challenge);

                            bet.ChallengeId = challenge.ChallengeId;
                            bet.BetStatus = bet.BetStatus.AsString();
                            bet.Challenge = null;
                            this._betDataRepository.Insert(bet);

                            scope.Complete();
                        }
                    }
                    else
                    {
                        bet.BetStatus = bet.BetStatus.AsString();
                        bet.Challenge = null;
                        this._betDataRepository.Insert(bet);
                    }

                    return new JsonActionResult(bet);
                }
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
                ex.ExceptionValueTracker();
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
                ex.ExceptionValueTracker(challengeId);
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
                ex.ExceptionValueTracker(userKey);
            }
            return null;
        }
        #endregion
    }
}
