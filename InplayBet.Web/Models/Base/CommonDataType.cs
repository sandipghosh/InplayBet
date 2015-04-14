
namespace InplayBet.Web.Models.Base
{
    public enum StatusCode
    {
        Active = 1,
        Inative = 2,
        Discontinue = 3,
        Won = 4,
        Lost = 5,
        CashOut = 6
    }

    public enum BetDisplayType
    {
        Insert = 1,
        Update = 2,
        Read = 3
    }

    public enum FollowType
    {
        NotApplicable = 0,
        Follow = 1,
        UnFollow = 2
    }

    public enum DisplayProperty
    {
        Name,
        Description
    }

    public enum SexType
    {
        Male,
        Female
    }
}
