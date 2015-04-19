

namespace InplayBet.Web.Data.Interface
{
    using InplayBet.Web.Data.Interface.Base;
    using InplayBet.Web.Models;

    public interface IBetDataRepository : IRepository<BetModel>
    {
        int GetConsicutiveBetWins(int userId);

        BetModel InsertBet(BetModel bet);

        BetModel UpdateBet(BetModel bet);
    }
}
