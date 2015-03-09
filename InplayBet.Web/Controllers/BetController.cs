

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using MoreLinq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;

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
        public ActionResult GetChallengesByUser(int userKey)
        {
            try
            {
                var challenges = GetChallenges(userKey)
                    .OrderByDescending(x => x.ChallengeId).ToList();
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
        public ActionResult GetBetsByChallenge(int challengeId)
        {
            try
            {
                var challenges = GetBets(challengeId);
                return View(challenges);
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
        public ActionResult ShowNewBetWindow(int userKey)
        {
            try
            {
                BetModel bet = new BetModel()
                {
                    Challenge = GetRecentChallenge(userKey),
                    StatusId = (int)StatusCode.Active,
                    CreatedBy = userKey,
                    CreatedOn = DateTime.Now
                };
                bet.BetNumber = GetBetNumber(bet.ChallengeId);
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
        [HttpPost]
        public ActionResult InsertNewBet(BetModel bet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Action<BetModel, int> insertBet = (b, challengeId) =>
                    {
                        b.ChallengeId = challengeId;
                        this._betDataRepository.Insert(b);
                    };
                    if (bet.ChallengeId == 0)
                    {
                        TransactionOptions options = new TransactionOptions()
                        {
                            IsolationLevel = IsolationLevel.ReadCommitted,
                            Timeout = new TimeSpan(0, 1, 0)
                        };
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
                        {
                            this._challengeDataRepository.Insert(bet.Challenge);
                            insertBet(bet, bet.ChallengeId);
                            scope.Complete();
                        }
                    }
                    else
                    {
                        insertBet(bet, bet.ChallengeId);
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
        /// Gets the challenge.
        /// </summary>
        /// <returns></returns>
        private ChallengeModel GetRecentChallenge(int userKey)
        {
            ChallengeModel currentChallenge = new ChallengeModel();
            try
            {
                Func<List<ChallengeModel>, int, ChallengeModel> getLatChellenge =
                    (c, id) => c.FirstOrDefault(x => x.ChallengeId.Equals(id));

                Func<int, ChallengeModel> createChallengeInstance = (num) =>
                    new ChallengeModel
                    {
                        ChallengeNumber = num,
                        StatusId = (int)StatusCode.Active,
                        CreatedBy = userKey,
                        CreatedOn = DateTime.Now
                    };

                var challenges = GetChallenges(userKey);
                if (challenges.Count == 0)
                {
                    return createChallengeInstance(1);
                }
                else
                {
                    BetModel bet = challenges.MaxBy(x => x.ChallengeId).Bets
                        .Where(x => x.StatusId.Equals((int)StatusCode.Active))
                        .MaxBy(x => x.BetId);

                    if (bet.BetStatus.AsString().ToLower() == "lost" ||
                        bet.BetStatus.AsString().ToLower() == "won")
                    {
                        ChallengeModel challenge = getLatChellenge(challenges, bet.ChallengeId);
                        return createChallengeInstance(challenge.ChallengeNumber + 1);
                    }
                    else
                    { return getLatChellenge(challenges, bet.ChallengeId); }
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
                BetModel lastBet = GetBets(challengeId).MaxBy(x => x.BetId);

                if (lastBet == null)
                    return 1;
                else
                    return lastBet.BetNumber + 1;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(challengeId);
            }
            return 0;
        }

        /// <summary>
        /// Gets the bets.
        /// </summary>
        /// <param name="challengeId">The challenge identifier.</param>
        /// <returns></returns>
        private List<BetModel> GetBets(int challengeId)
        {
            try
            {
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
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        private List<ChallengeModel> GetChallenges(int userId)
        {
            try
            {
                return this._challengeDataRepository.GetList(x => x.UserKey.Equals(userId)
                    && x.StatusId.Equals((int)StatusCode.Active)).ToList();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userId);
            }
            return null;
        }
    }
}
