using GymGenius.Models.Identity;
using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymGenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }


        //[Authorize(Roles = clsRoles.roleAdmin)]
        [HttpPost("registerwithRole")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModelForAdminandCoach model, string role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.RegisterAsync(model, role);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string Token, [FromQuery] string Email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.ConfirmEmailAsync(Email,Token);

            return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authRepository.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        #region Forget Password

        [HttpPost("SendResetPassword")]
        public async Task<IActionResult> SendResetPassword([FromQuery] string Email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.SendResetPasswordAsync(Email);

            return Ok(result);
        }


        [HttpGet("ConfirmResetPassword")]
        public async Task<IActionResult> ConfirmResetPassword([FromQuery] string Code, [FromQuery] string Email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.ConfirmResetPasswordAsync(Code, Email);

            return Ok(result);
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] string Email, [FromForm] string Password)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.ResetPasswordAsync(Email, Password);

            return Ok(result);
        }

        #endregion


        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
