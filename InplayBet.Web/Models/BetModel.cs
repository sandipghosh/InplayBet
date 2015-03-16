

namespace InplayBet.Web.Models
{
    using System;
    using AutoMapper;
    using InplayBet.Web.Models.Base;
    using System.ComponentModel.DataAnnotations;

    public class BetModel : BaseModel
    {
        public int BetId { get; set; }
        public int BetNumber { get; set; }
        public int TeamAId { get; set; }
        public int TeamAName { get; set; }
        public int TeamBId { get; set; }
        public int TeamBName { get; set; }
        public int LegueId { get; set; }
        public int ChallengeId { get; set; }
        public string BetType { get; set; }
        public string Odds { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal BetPlaced { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal WiningTotal { get; set; }

        public decimal LoosingTotal { get; set; }
        public string BetStatus { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [IgnoreMap]
        public virtual ChallengeModel Challenge { get; set; }
        public virtual LegueModel Legue { get; set; }
        public virtual TeamModel TeamA { get; set; }
        public virtual TeamModel TeamB { get; set; }
    }
}