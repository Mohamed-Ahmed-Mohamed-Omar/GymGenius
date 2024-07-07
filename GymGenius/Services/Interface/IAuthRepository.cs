using GymGenius.Models.Identity;

namespace GymGenius.Services.Interface
{
    public interface IAuthRepository
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);

        Task<AuthModel> RegisterAsync(RegisterModelForAdminandCoach model, string RoleName);

        Task<AuthModel> LoginAsync(LoginModel model);

        Task<string> ConfirmEmailAsync(string Email, string Token);

        Task<string> SendResetPasswordAsync(string Email);

        Task<string> ConfirmResetPasswordAsync(string Code, string Email);

        Task<string> ResetPasswordAsync(string Email, string Password);
    }
}
