

namespace InplayBet.Web.Models
{
    using InplayBet.Web.Models.Base;
    using System;

    public class ReportModel : BaseModel
    {
        public int ReportId { get; set; }
        public int ReportedBy { get; set; }
        public int ReportedUserId { get; set; }
        public int ReportedChallengeId { get; set; }
        public string Comment { get; set; }
        public string ReportStatus { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ChallengeModel Challenge { get; set; }
        public virtual UserModel UserTo { get; set; }
        public virtual UserModel UserBy { get; set; }
    }
}