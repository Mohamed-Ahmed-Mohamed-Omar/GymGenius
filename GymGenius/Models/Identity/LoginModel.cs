using System.ComponentModel.DataAnnotations;

namespace GymGenius.Models.Identity
{
    public class LoginModel
    {
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
    }
}
