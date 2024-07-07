using GymGenius.Models.Identity;
using GymGenius.Models.TrackProgresses;
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
    public class TrackProgressController : ControllerBase
    {
        private readonly ITrackProgressRepository _trackProgress;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrackProgressController(ITrackProgressRepository trackProgress, UserManager<ApplicationUser> userManager)
        {
            _trackProgress = trackProgress;
            _userManager = userManager;
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpPost("CreateProgress")]
        public async Task<IActionResult> CreateProgress([FromBody] CreateTrackProgress createTrack)
        {
            try
            {
                var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByNameAsync(username);

                var data = await _trackProgress.AddAsync(createTrack, user.UserName);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("Faild Add");
            }
        }


        [Authorize]
        [HttpGet("GetAllLevel")]
        public async Task<ActionResult<IEnumerable<int>>> GetAllLevel()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(username);

            var data = await _trackProgress.GeTAllLevelAsync(user.UserName);

            return Ok(data);
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpGet("GetTrackProgress")]
        public async Task<ActionResult<IEnumerable<GetAllTrackProgress>>> GetTrackProgress()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(username);

            var data = await _trackProgress.GetAllTrackProgressesAsync(user.UserName);

            return Ok(data);
        }


        [Authorize(Roles = clsRoles.roleCoach + "," + clsRoles.roleAdmin)]
        [HttpGet("GetTrackProgress/{UserName}")]
        public async Task<ActionResult<IEnumerable<GetAllTrackProgress>>> GetTrackProgress(string UserName)
        {
            var data = await _trackProgress.GetAllTrackProgressesAsync(UserName);

            return Ok(data);
        }
    }
}
