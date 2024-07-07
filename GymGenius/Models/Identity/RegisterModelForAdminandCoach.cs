using System.ComponentModel.DataAnnotations;

namespace GymGenius.Models.Identity
{
    public class RegisterModelForAdminandCoach
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public int Age { get; set; }

        public float? Salary { get; set; }

        public DateTime Start { get; set; } = DateTime.Now;
    }
}
