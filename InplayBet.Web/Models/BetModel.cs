

namespace InplayBet.Web.Models
{
    using AutoMapper;
    using InplayBet.Web.Models.Base;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    public class BetModel : BaseModel
    {
        public int BetId { get; set; }
        public int BetNumber { get; set; }
        public int TeamAId { get; set; }
        public string TeamAName { get; set; }
        public int TeamBId { get; set; }
        public string TeamBName { get; set; }
        public int LegueId { get; set; }
        public string LegueName { get; set; }
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

        public string UserId { get; set; }

        public int ChallengeNumber { get; set; }
        public string BetInfoUrl
        {
            get
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return urlHelper.Action("Bet", "BetInfo", new
                {
                    userKey = this.CreatedBy,
                    betId = this.BetId
                }, HttpContext.Current.Request.Url.Scheme);
            }
        }
    }
}