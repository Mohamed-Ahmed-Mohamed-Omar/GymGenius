using GymGenius.Models.Identity;
using GymGenius.Models.Rates;
using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymGenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateRepository _rateRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public RateController(IRateRepository rateRepository, UserManager<ApplicationUser> userManager)
        {
            _rateRepository = rateRepository;
            _userManager = userManager;
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpPost("CreateRate")]
        public async Task<IActionResult> CreateRate([FromBody] CreateRate rate)
        {
            if (ModelState.IsValid)
            {
                var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByNameAsync(username);

                var data = await _rateRepository.AddAsync(rate, user.UserName);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize]
        [HttpGet("GelAllRateTheCoach/{CoachName}")]
        public async Task<ActionResult<IEnumerable<GetAllRates>>> GetAllRateCoachName(string CoachName)
        {
            var data = await _rateRepository.GetAllAsync(CoachName);

            return Ok(data);
        }
    }
}
