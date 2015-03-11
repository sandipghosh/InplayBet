

namespace InplayBet.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using InplayBet.Web.Models.Base;

    public class UserModel : BaseModel
    {
        public int UserKey { get; set; }
        public string UserId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fillup First Name")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter email address")]
        public string EmailId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter password"),
        RegularExpression(@"^[a-zA-Z\d]{8,}$", ErrorMessage = "Please enter a valid password")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter cpnfirm password"),
        Compare("Password", ErrorMessage = "Please enter correct cpnfirm password")]
        public string ConfirmPassword { get; set; }
        public string Sex { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter day")]
        public int DobDay { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter month")]
        public int DobMonth { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter year")]
        public int DobYear { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int BookMakerId { get; set; }
        public string AvatarPath { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual BookMakerModel BookMaker { get; set; }
        public virtual IEnumerable<ChallengeModel> Challenges { get; set; }
    }
}