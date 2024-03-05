using GymGenius.Models.Notifications;
using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymGenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpGet("GetAllNotifications")]
        public async Task<ActionResult<IEnumerable<GetAllNotifications>>> GetAllNotifications()
        {
            if (ModelState.IsValid)
            {
                var data = await _notificationRepository.ListAllAsync();

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpPost("CreateNotification")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotification notification)
        {
            if (ModelState.IsValid)
            {
                var data = await _notificationRepository.AddAsync(notification);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpDelete("DeleteNotification{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (ModelState.IsValid)
            {
                var data = await _notificationRepository.DeleteAsync(id);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }
    }
}
