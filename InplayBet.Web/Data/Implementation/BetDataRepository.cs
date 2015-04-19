
namespace InplayBet.Web.Data.Implementation
{
    using InplayBet.Web.Data.Context;
    using InplayBet.Web.Data.Implementation.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Data.Interface.Base;
    using InplayBet.Web.Models;
    using InplayBet.Web.Utilities;
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    public class BetDataRepository : DataRepository<Bet, BetModel>, IBetDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public BetDataRepository(UnitOfWork<InplayBetDBEntities> unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the consicutive bet wins.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public int GetConsicutiveBetWins(int userId)
        {
            try
            {
                IQueryDataRepository query = new QueryDataRepository<InplayBetDBEntities>();

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@userid", userId)
                };
                var result = query.ExecuteQuery<int>("EXEC ConsecutiveBetByUser @userid", param);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return 0;
        }

        /// <summary>
        /// Inserts the bet.
        /// </summary>
        /// <param name="bet">The bet.</param>
        /// <returns></returns>
        public BetModel InsertBet(BetModel bet)
        {
            try
            {
                IQueryDataRepository query = new QueryDataRepository<InplayBetDBEntities>();
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@BetId", bet.BetId),
                    new SqlParameter("@BetNumber", bet.BetNumber),
                    new SqlParameter("@TeamAName", bet.TeamAName),
                    new SqlParameter("@TeamBName", bet.TeamBName),
                    new SqlParameter("@LegueName", bet.LegueName),
                    new SqlParameter("@ChallengeId", bet.ChallengeId),
	                new SqlParameter("@ChallengeNumber", bet.ChallengeNumber),
                    new SqlParameter("@BetType", bet.BetType),
                    new SqlParameter("@Odds", bet.Odds),
                    new SqlParameter("@BetPlaced", bet.BetPlaced),
                    new SqlParameter("@WiningTotal", bet.WiningTotal),
                    new SqlParameter("@LoosingTotal", bet.LoosingTotal),
                    new SqlParameter("@BetStatus", bet.BetStatus.AsString()),
                    new SqlParameter("@StatusId", bet.StatusId),
                    new SqlParameter("@CreatedBy", bet.CreatedBy)
                };
                var result = query.ExecuteQuery<BetModel>("EXEC InsertBet @BetId, @BetNumber, @TeamAName, @TeamBName, @LegueName, @ChallengeId, @ChallengeNumber, @BetType, @Odds, @BetPlaced, @WiningTotal, @LoosingTotal, @BetStatus, @StatusId, @CreatedBy", param);
                var newBet = result.FirstOrDefault();
                return newBet;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bet);
            }
            return bet;
        }

        /// <summary>
        /// Inserts the bet.
        /// </summary>
        /// <param name="bet">The bet.</param>
        /// <returns></returns>
        public BetModel UpdateBet(BetModel bet)
        {
            try
            {
                IQueryDataRepository query = new QueryDataRepository<InplayBetDBEntities>();
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@BetId", bet.BetId),
                    new SqlParameter("@ChallengeId", bet.ChallengeId),
                    new SqlParameter("@WiningTotal", bet.WiningTotal),
                    new SqlParameter("@WiningLimitAmountOfChallenge", CommonUtility.GetConfigData<decimal>("WiningBetAmount")),
                    new SqlParameter("@StartingBetAmount", CommonUtility.GetConfigData<decimal>("StartingBetAmount")),
                    new SqlParameter("@BetStatus", bet.BetStatus.AsString()),
                };
                var result = query.ExecuteQuery<BetModel>("EXEC UpdateBet @BetId, @ChallengeId, @WiningTotal, @WiningLimitAmountOfChallenge, @StartingBetAmount, @BetStatus", param);
                var newBet = result.FirstOrDefault();
                return newBet;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bet);
            }
            return bet;
        }
    }
}