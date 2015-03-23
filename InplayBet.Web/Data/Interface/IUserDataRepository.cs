
namespace InplayBet.Web.Data.Interface
{
    using InplayBet.Web.Data.Interface.Base;
    using InplayBet.Web.Models;

    public interface IUserDataRepository : IRepository<UserModel>
    {
        void ResetAccount(int userKey);
    }
}
