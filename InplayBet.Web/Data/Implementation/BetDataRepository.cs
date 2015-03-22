
namespace InplayBet.Web.Data.Implementation
{
    using InplayBet.Web.Data.Context;
    using InplayBet.Web.Data.Implementation.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Data.Interface.Base;
    using InplayBet.Web.Models;
    using InplayBet.Web.Utilities;
    using System;
    using System.Linq;
    using System.Data.SqlClient;

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
    }
}