
namespace InplayBet.Web.Models
{
    using InplayBet.Web.Models.Base;
    using System;

    public class FollowModel : BaseModel
    {
        public int FollowId { get; set; }
        public int FollowBy { get; set; }
        public int FollowTo { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}