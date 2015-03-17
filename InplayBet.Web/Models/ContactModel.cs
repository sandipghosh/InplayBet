

namespace InplayBet.Web.Models
{
    using InplayBet.Web.Models.Base;
    using System;

    public class ContactModel : BaseModel
    {
        public int CotactUsId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string Massage { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}