using InplayBet.Web.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace InplayBet.Web.Models
{
    public class UserRankViewModel : BaseModel
    {
        public int UserKey { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        public System.DateTime MemberSince { get; set; }
        public int CurrencyId { get; set; }
        public string CultureCode { get; set; }
        public string CurrencySymbol { get; set; }
        public int BookMakerId { get; set; }
        public string BookMakerName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public decimal Won { get; set; }
        public decimal Placed { get; set; }
        public decimal Profit { get; set; }
        public long Rank { get; set; }
        public string WinningBets { get; set; }
    }
}