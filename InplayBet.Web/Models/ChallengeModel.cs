﻿
namespace InplayBet.Web.Models
{
    using System;
    using System.Collections.Generic;
    using InplayBet.Web.Models.Base;
    using AutoMapper;

    public class ChallengeModel : BaseModel
    {
        public int ChallengeId { get; set; }
        public int ChallengeNumber { get; set; }
        public int UserKey { get; set; }
        public decimal WiningPrice { get; set; }
        public string ChallengeStatus { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual IEnumerable<BetModel> Bets { get; set; }

        [IgnoreMap]
        public UserModel User { get; set; }
    }
}