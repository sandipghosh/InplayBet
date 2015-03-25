

namespace InplayBet.Web.Models
{
    using InplayBet.Web.Models.Base;
    using System.ComponentModel.DataAnnotations;
    using System;

    public class ContactModel : BaseModel
    {
        public int CotactUsId { get; set; }
        [Display(Name = "Your Name"),
        Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [Display(Name = "Your Email Address"),
        Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Email Address"),
        RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter a Valid Email Address")]
        public string EmailId { get; set; }
        [Display(Name = "Your Phone Number"),
        Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Your Message"),
        Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Message"),
        StringLength(500, MinimumLength = 100, ErrorMessage = "Massage lenght must be 100 to 500 cheracters")]
        public string Massage { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}