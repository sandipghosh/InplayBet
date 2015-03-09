

namespace InplayBet.Web.Models
{
    using System;
    using InplayBet.Web.Models.Base;

    public class LegueModel : BaseModel
    {
        public int LegueId { get; set; }
        public string LegueName { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}