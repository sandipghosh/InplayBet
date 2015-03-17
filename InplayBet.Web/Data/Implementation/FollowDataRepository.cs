﻿
namespace InplayBet.Web.Data.Implementation
{
    using InplayBet.Web.Data.Context;
    using InplayBet.Web.Data.Implementation.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;

    public class FollowDataRepository : DataRepository<Follow, FollowModel>, IFollowDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public FollowDataRepository(UnitOfWork<InplayBetDBEntities> unitOfWork)
            :base(unitOfWork)
        {
        }
    }
}