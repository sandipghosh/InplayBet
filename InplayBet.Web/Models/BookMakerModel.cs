
namespace InplayBet.Web.Models
{
    using InplayBet.Web.Models.Base;
    using System;

    public class BookMakerModel : BaseModel
    {
        public int BookMakerId { get; set; }
        public string BookMakerName { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}