using GymGenius.Helpers;
using GymGenius.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace GymGenius.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(IHubContext<ChatHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _hubContext = hubContext;
            _userManager = userManager;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(string message)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (username == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);

            if(user.ProfilePhoroUrl != null)
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", user.ProfilePhoroUrl, message);
            else
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", user.UserName, message);

            return Ok();
        }
    }
}
