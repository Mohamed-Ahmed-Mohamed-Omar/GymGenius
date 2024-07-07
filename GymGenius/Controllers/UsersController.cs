using GymGenius.Models.Identity;
using GymGenius.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymGenius.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet("ShowProfile")]
        public async Task<IActionResult> ShowProfile()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (username == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);

            var profile = new ProFileVM
            {
                Age = user.Age,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoUrl = user.ProfilePhoroUrl,
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                Salary = user.Salary
            };

            return Ok(profile);
        }


        [HttpPost("updateProfile")]
        public async Task<IActionResult> updateProfile([FromBody] ProFileVM model)
        {
            // Get The Current User
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (username == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            // Save previous values
            var previousEmail = user.Email;
            var previousUserName = user.UserName;
            var previousFirstName = user.FirstName;
            var previousLastName = user.LastName;
            var previousSalary = user.Salary;
            var previousProfilePhoroUrl = user.ProfilePhoroUrl;
            var previousAge = user.Age;


            // Update Email And Check Is Not Null
            if (!string.IsNullOrEmpty(model.Email))
            {
                user.Email = model.Email;

                var UserWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

                if (UserWithSameEmail != null)
                {
                    ModelState.AddModelError("Email", "This email is already assigned to another user");

                    return BadRequest(model);
                }
            }


            // Update UserName And Check Is Not Null
            if (!string.IsNullOrEmpty(model.UserName))
            {
                user.UserName = model.UserName;

                var UserWithSameUserName = await _userManager.FindByNameAsync(model.UserName);

                if (UserWithSameUserName != null)
                {
                    ModelState.AddModelError("UserName", "This UserName is already assigned to another user");

                    return BadRequest(model);
                }
            }


            // Update FirstName And Check Is Not Null
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                user.FirstName = model.FirstName;
            }


            // Update LastName And Check Is Not Null
            if (!string.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }

            // Update Phone And Check Is Not Null
            if (!string.IsNullOrEmpty(model.Phone))
            {
                user.PhoneNumber = model.Phone;
            }


            // Update Salary And Check Is Not Null
            if (model.Salary != default(float))
            {
                user.Salary = model.Salary ?? previousAge;
            }


            // Update ProfilePhoroUrl And Check Is Not Null
            if (!string.IsNullOrEmpty(model.PhotoUrl))
            {
                user.ProfilePhoroUrl = model.PhotoUrl;
            }


            // Update Age And Check Is Not Null
            if (model.Age != default(int))
            {
                user.Age = model.Age ?? previousAge;
            }


            // Update Password And Check Is Not Null
            if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.OldPassword))
            {
                // Verify the validity of the old password
                var passwordValid = await _userManager.CheckPasswordAsync(user, model.OldPassword);

                if (passwordValid)
                {
                    // The old password is correct, so we update the new password
                    var newPassword = model.NewPassword;

                    // Check if the new password meets Microsoft's default password requirements
                    var passwordOptions = new PasswordOptions
                    {
                        RequiredLength = 8, // Minimum length
                        RequiredUniqueChars = 1, // Minimum different characters
                        RequireDigit = true, // Require a digit
                        RequireLowercase = true, // Require a lowercase letter
                        RequireNonAlphanumeric = true, // Require a non-alphanumeric character
                        RequireUppercase = true // Require an uppercase letter
                    };

                    var passwordErrors = new List<IdentityError>();

                    // Validate the new password
                    var isValidPassword = _userManager.PasswordValidators.All(v => v.ValidateAsync(_userManager, null, newPassword).Result.Succeeded);

                    if (!isValidPassword)
                    {
                        // The new password does not meet the requirements
                        var errors = _userManager.PasswordValidators.Select(v => v.ValidateAsync(_userManager, null, newPassword).Result.Errors);
                        foreach (var error in errors)
                        {
                            passwordErrors.AddRange(error);
                        }

                        return BadRequest(passwordErrors.Select(error => error.Description).ToList());
                    }

                    // Hash the new password
                    var hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, newPassword);

                    // Update the user's password hash
                    user.PasswordHash = hashedNewPassword;

                    model.NewPassword = "Update Password Succeefully";
                }
                else
                {
                    return BadRequest("Old password is incorrect!");
                }
            }

            await _userManager.UpdateAsync(user);


            // Update the model with the previous values for unchanged fields
            var auth = new ProFileVM
            {
                Age = user.Age,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoUrl = user.ProfilePhoroUrl,
                Salary = user.Salary,
                UserName = user.UserName,
                NewPassword = model.NewPassword,
                Phone = model.Phone
            };

            return Ok(auth);
        }
    }
}