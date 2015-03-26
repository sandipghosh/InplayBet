

namespace InplayBet.Web.Models
{
    using InplayBet.Web.Models.Base;
    using System.ComponentModel.DataAnnotations;

    public class ReportCountViewModel : BaseModel
    {
        [Display(Name = "User Id")]
        public string UserId { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Report Count")]
        public int ReportCount { get; set; }
    }
}