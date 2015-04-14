

namespace InplayBet.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using InplayBet.Web.Models.Base;

    public class UserRankViewModel : BaseModel
    {
        public int UserKey { get; set; }
        [Display(Name = "Member Number")]
        public string UserId { get; set; }
        [Display(Name = "Name")]
        public string UserName { get; set; }
        public string Address { get; set; }
        public string AvatarPath { get; set; }
        [Display(Name = "Date Joined")]
        public DateTime MemberSince { get; set; }
        public int CurrencyId { get; set; }
        public string CultureCode { get; set; }
        public string CurrencySymbol { get; set; }
        public int BookMakerId { get; set; }
        [Display(Name = "Bookmaker")]
        public string BookMakerName { get; set; }
        public int Followers { get; set; }
        public int Followings { get; set; }
        public int BetWins { get; set; }
        [Display(Name = "Won")]
        public int Wins { get; set; }
        [Display(Name = "Lost")]
        public int Losses { get; set; }
        public decimal Won { get; set; }
        public decimal ActualWon { get; set; }
        public decimal Placed { get; set; }
        public decimal Profit { get; set; }
        public long Rank { get; set; }
        public string WinningBets { get; set; }
        public string Sex { get; set; }
        [Display(Name = "Total Bets")]
        public int TotalBets { get; set; }
        [Display(Name = "Total Challenges")]
        public int TotalChallenges { get; set; }
        [Display(Name = "Total Cheat Reported")]
        public int TotalCheatReported { get; set; }
    }
}