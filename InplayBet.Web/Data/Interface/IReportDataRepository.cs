
namespace InplayBet.Web.Data.Interface
{
    using System.Collections.Generic;
    using InplayBet.Web.Data.Interface.Base;
    using InplayBet.Web.Models;

    public interface IReportDataRepository : IRepository<ReportModel>
    {
        List<ReportCountViewModel> GetCheatCountByUser(int reportedUserId);
    }
}
