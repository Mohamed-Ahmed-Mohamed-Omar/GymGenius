using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;

namespace GymGenius.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }

        [EncryptColumn]
        public string? Code { get; set; }

        public string? ProfilePhoroUrl { get; set; }

        public int Age { get; set; } = default;

        public float? Salary { get; set; }

        public DateTime Start { get; set; } 
    }
}
