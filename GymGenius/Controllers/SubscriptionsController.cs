using GymGenius.Models.Identity;
using GymGenius.Models.Subscriptions;
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
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IRateRepository _rateRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionsController(ISubscriptionRepository subscriptionRepository, UserManager<ApplicationUser> userManager, IRateRepository rateRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _userManager = userManager;
            _rateRepository = rateRepository;
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpGet("IsVaild")]
        public async Task<IActionResult> IsVaild()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(username);

            var data = await _subscriptionRepository.IsAvailable(user.UserName);

            return Ok(data);
        }


        [Authorize(Roles = clsRoles.roleCoach + "," + clsRoles.roleAdmin)]
        [HttpGet("GetAllSubscriptions")]
        public async Task<ActionResult<IEnumerable<GetSubscriptionDetails>>> GetAllSubscriptions()
        {
            if (ModelState.IsValid)
            {
                var data = await _subscriptionRepository.ListAllAsync();

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpGet("GetAllTraineesForCoach")]
        public async Task<IActionResult> GetAllTraineesForCoach()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(username);

            var data = await _subscriptionRepository.GetAllTrainnes(user.UserName);

            if (data == null)
            {
                return BadRequest($"Not Found Trainees For This: {user.UserName}");
            }
            else
                return Ok(data);
        }


        [Authorize(Roles = clsRoles.roleAdmin)]
        [HttpGet("GetAllTrainees/{CoachName}")]
        public async Task<IActionResult> GetAllTrainees(string CoachName)
        {
            var data = await _subscriptionRepository.GetAllTrainnes(CoachName);

            return Ok(data);
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpGet("GetSubscriptionDetailsToUser")]
        public async Task<IActionResult> GetSubscriptionDetailsToUser()
        {
            if (ModelState.IsValid)
            {

                var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByNameAsync(username);

                var data = await _subscriptionRepository.GetByIdAsync(user.UserName);

                if (data == null)
                {
                    return BadRequest($"Not Found : {user.UserName}");
                }
                else
                    return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [HttpGet("GetAllCoaches")]
        public async Task<IActionResult> GetAllCoaches()
        {
            var userRole = await _userManager.GetUsersInRoleAsync(clsRoles.roleCoach);
            if (userRole == null)
            {
                return NotFound($"No users found in role {clsRoles.roleCoach}.");
            }

            var users = userRole.Select(user => new
            {
                Username = user.UserName,
                Age = user.Age,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Start = user.Start
            }).ToList();

            return Ok(users);
        }


        [Authorize(Roles = clsRoles.roleAdmin)]
        [HttpGet("GetAllCoachesDetails")]
        public async Task<IActionResult> GetAllCoachesDetails()
        {
            var userRole = await _userManager.GetUsersInRoleAsync(clsRoles.roleCoach);
            if (userRole == null)
            {
                return NotFound($"No users found in role {clsRoles.roleCoach}.");
            }

            var users = userRole.Select(user => new
            {
                Username = user.UserName,
                Age = user.Age,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.FirstName + " " + user.LastName,
                TraineesNumber = _subscriptionRepository.GetAllTrainnes(user.UserName).Result.Count(),
                Salary = user.Salary,
                Start = user.Start
            }).ToList();

            return Ok(users);
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpPost("CreateSubscription")]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscription subscription)
        {
            if (ModelState.IsValid)
            {
                var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByNameAsync(username);

                var UserName = user.UserName;

                int age = user.Age;

                var data = await _subscriptionRepository.AddAsync(subscription, UserName, age);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleCoach + "," + clsRoles.roleUser + "," + clsRoles.roleAdmin)]
        [HttpDelete("DeleteSubscription/{Id}")]
        public async Task<IActionResult> DeleteSubscription(int Id)
        {
            var data = await _subscriptionRepository.DeleteAsync(Id);

            return Ok(data);
        }


        [Authorize(Roles = clsRoles.roleCoach + "," + clsRoles.roleAdmin)]
        [HttpPut("UpdateSubscription/{id}")]
        public async Task<IActionResult> UpdateSubscription(int id, [FromBody] UpdateSubscription subscription)
        {
            subscription.Id = id; // Ensure Id matches update target

            try
            {
                var updatedSubscription = await _subscriptionRepository.UpdateAsync(subscription);

                return Ok(updatedSubscription);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Handle offer not found gracefully
            }
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpPut("UpdateSubscriptionToUser")]
        public async Task<IActionResult> UpdateSubscriptionToUser([FromBody] UpdateSubscriptionToUser subscription)
        {

            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(username);

            var UserName = user.UserName;

            subscription.UserName = UserName; // Ensure UserName matches update target

            try
            {
                var updatedSubscription = await _subscriptionRepository.UpdateAsync(subscription);

                return Ok(updatedSubscription);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Handle offer not found gracefully
            }
        }

    }
}
