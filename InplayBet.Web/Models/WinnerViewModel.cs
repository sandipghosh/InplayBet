
namespace InplayBet.Web.Models
{
    using InplayBet.Web.Models.Base;

    public class WinnerViewModel : BaseModel
    {
        public UserRankViewModel User { get; set; }
        public ChallengeModel WonChallenge { get; set; }
    }
}