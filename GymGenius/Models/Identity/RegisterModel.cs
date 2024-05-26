using System.ComponentModel.DataAnnotations;

namespace GymGenius.Models.Identity
{
    public class RegisterModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
