namespace InplayBet.Web.Data.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using InplayBet.Web.Data.Context;
    using InplayBet.Web.Data.Implementation.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Data.Interface.Base;
    using InplayBet.Web.Models;
    using InplayBet.Web.Utilities;

    public class ReportDataRepository : DataRepository<Report, ReportModel>, IReportDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReportDataRepository(UnitOfWork<InplayBetDBEntities> unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the cheat count by user.
        /// </summary>
        /// <param name="reportedUserId">The reported user id.</param>
        /// <returns></returns>
        public List<ReportCountViewModel> GetCheatCountByUser(int reportedUserId)
        {
            try
            {
                IQueryDataRepository query = new QueryDataRepository<InplayBetDBEntities>();
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ReportedUserId", reportedUserId)
                };
                var result = query.ExecuteQuery<ReportCountViewModel>("EXEC GetCheatCountByUser @ReportedUserId", param);
                return result.ToList();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(reportedUserId);
            }
            return new List<ReportCountViewModel>();
        }
    }
}