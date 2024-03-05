using System.ComponentModel.DataAnnotations;

namespace GymGenius.Models.Identity
{
    public class ResetPassword
    {
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The Password and Confirm Password do not match. ")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
