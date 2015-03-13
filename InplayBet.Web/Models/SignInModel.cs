
namespace InplayBet.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SignInModel
    {
        [Required(), Display(Name = "User Id or Email Id")]
        public string UserOrEmail { get; set; }
        [Required(), Display(Name = "Password")]
        public string Password { get; set; }
    }
}