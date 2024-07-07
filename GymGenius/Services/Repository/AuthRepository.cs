using GymGenius.Data;
using GymGenius.Helpers;
using GymGenius.Models.Identity;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GymGenius.Services.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailingRepository _mailingRepository;
        private readonly ApplicationDbContext _Context;
        private readonly IUrlHelper _urlHelper;
        private readonly JWT _jwt;

        public AuthRepository(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IMailingRepository mailingRepository, ApplicationDbContext context, IUrlHelper urlHelper, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mailingRepository = mailingRepository;
            _Context = context;
            _urlHelper = urlHelper;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            var trans = await _Context.Database.BeginTransactionAsync();
            try
            {
                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    return (new AuthModel { Message = "Email is already registered!" });

                if (await _userManager.FindByNameAsync(model.Username) is not null)
                    return (new AuthModel { Message = "Username is already registered!" });

                var Id = GenerateRandomID();

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                user.Id = Id;

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    return (new AuthModel { Message = errors });
                }

                await _userManager.AddToRoleAsync(user, "User");

                var jwtSecurityToken = await CreateJwtToken(user);

                var refreshToken = GenerateRefreshToken();

                //Send Confirm Email

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var requestAccessor = _httpContextAccessor.HttpContext.Request;

                var returnUrl = requestAccessor.Scheme + "://" + requestAccessor.Host + _urlHelper.Action("ConfirmEmail", "Auth", new { Token = code, Email = user.Email });

                var message = $"To Confirm Email Click Link: <a href='{returnUrl}'>Link Of Confirmation</a>";

                var sent = await _mailingRepository.SendingMail(user.Email, message, "Confirm Email");

                if (sent != "Success")
                {
                    return (new AuthModel { Message = "Email Not Confirm!" });
                }

                user.RefreshTokens?.Add(refreshToken);
                await _userManager.UpdateAsync(user);

                await trans.CommitAsync();
                return (new AuthModel
                {
                    Email = user.Email,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    IsAuthenticated = true,
                    Roles = new List<string> { "User" },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Username = user.UserName,
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiration = refreshToken.ExpiresOn
                });
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return (new AuthModel
                {
                    Message = ex.Message
                });
            }
        }

        public async Task<AuthModel> RegisterAsync(RegisterModelForAdminandCoach model, string Role)
        {
            var trans = await _Context.Database.BeginTransactionAsync();
            try
            {
                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    return (new AuthModel { Message = "Email is already registered!" });

                if (await _userManager.FindByNameAsync(model.Username) is not null)
                    return (new AuthModel { Message = "Username is already registered!" });

                var Id = GenerateRandomID();

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    PhoneNumber = model.Phone,
                    Salary = model.Salary,
                    Start = DateTime.UtcNow
                };

                user.Id = Id;

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    return (new AuthModel { Message = errors });
                }

                if (await _roleManager.RoleExistsAsync(Role))
                    await _userManager.AddToRoleAsync(user, Role);
                else
                    return (new AuthModel { Message = "InVaild Role" });

                var jwtSecurityToken = await CreateJwtToken(user);

                var refreshToken = GenerateRefreshToken();

                //Send Confirm Email

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var requestAccessor = _httpContextAccessor.HttpContext.Request;

                var returnUrl = requestAccessor.Scheme + "://" + requestAccessor.Host + _urlHelper.Action("ConfirmEmail", "Auth", new { Token = code, Email = user.Email });

                var message = $"To Confirm Email Click Link: <a href='{returnUrl}'>Link Of Confirmation</a>";

                var sent = await _mailingRepository.SendingMail(user.Email, message, "Confirm Email");

                if (sent != "Success")
                {
                    return (new AuthModel { Message = "Email Not Confirm!" });
                }

                user.RefreshTokens?.Add(refreshToken);
                await _userManager.UpdateAsync(user);

                await trans.CommitAsync();
                return (new AuthModel
                {
                    Email = user.Email,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    IsAuthenticated = true,
                    Roles = new List<string> { Role },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Username = user.UserName,
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiration = refreshToken.ExpiresOn
                });
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return (new AuthModel
                {
                    Message = ex.Message
                });
            }
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.EmailOrUserName) ??
                        await _userManager.FindByNameAsync(model.EmailOrUserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return (authModel);
            }

            //if (!user.EmailConfirmed)
            //{
            //    var result = await _userManager.DeleteAsync(user);
            //    authModel.Message = "Email is not confirmed! And User deleted successfully!";
            //    return (authModel);
            //}

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return (authModel);
        }

        public async Task<string> ConfirmEmailAsync(string Email, string Token)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, Token);

                if (result.Succeeded)
                {
                    return ("Email is confirm");
                }
            }

            return ("No Email Sent Successfully");
        }

        public async Task<string> SendResetPasswordAsync(string Email)
        {
            var trans = await _Context.Database.BeginTransactionAsync();
            try
            {
                //user
                var user = await _userManager.FindByEmailAsync(Email);
                //user not Exist => not found
                if (user == null)
                    return "UserNotFound";
                //Generate Random Number

                //Random generator = new Random();
                //string randomNumber = generator.Next(0, 1000000).ToString("D6");
                var chars = "0123456789";
                var random = new Random();
                var randomNumber = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

                //update User In Database Code
                user.Code = randomNumber;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    return "ErrorInUpdateUser";

                var message = "Code To Reset Password : " + user.Code;

                //Send Code To  Email 
                await _mailingRepository.SendingMail(user.Email, message, "Reset Password");
                await trans.CommitAsync();
                return Email;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "Failed Send Code";
            }
        }

        public async Task<string> ConfirmResetPasswordAsync(string Code, string Email)
        {
            //Get User
            //user
            var user = await _userManager.FindByEmailAsync(Email);
            //user not Exist => not found
            if (user == null)
                return "UserNotFound";
            //Decrept Code From Database User Code
            var userCode = user.Code;
            //Equal With Code
            if (userCode == Code) return "Success";
            return "Failed";
        }

        public async Task<string> ResetPasswordAsync(string Email, string Password)
        {
            var trans = await _Context.Database.BeginTransactionAsync();
            try
            {
                //Get User
                var user = await _userManager.FindByEmailAsync(Email);
                //user not Exist => not found
                if (user == null)
                    return "UserNotFound";
                await _userManager.RemovePasswordAsync(user);
                if (!await _userManager.HasPasswordAsync(user))
                {
                    await _userManager.AddPasswordAsync(user, Password);
                }
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }

        #region Helpers

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }

        private static string GenerateRandomID()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string nums = "0123456789";

            StringBuilder stringBuilder = new StringBuilder();

            Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            for (int i = 0; i < 2; i++)
            {
                stringBuilder.Append(nums[random.Next(nums.Length)]);
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}
