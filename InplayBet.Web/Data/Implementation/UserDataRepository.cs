

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

    public class UserDataRepository : DataRepository<User, UserModel>, IUserDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserDataRepository(UnitOfWork<InplayBetDBEntities> unitOfWork)
            :base(unitOfWork)
        {
        }

        public void ResetAccount(int userKey)
        {
            try
            {
                IQueryDataRepository query = new QueryDataRepository<InplayBetDBEntities>();

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@userid", userKey)
                };
                var result = query.ExecuteCommand("EXEC ResetUserAccount @userid", param);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
        }
    }
}