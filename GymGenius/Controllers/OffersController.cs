using GymGenius.Models.Identity;
using GymGenius.Models.Offers;
using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymGenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOfferRepository _offerRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OffersController(IOfferRepository offerRepository, UserManager<ApplicationUser> userManager)
        {
            _offerRepository = offerRepository;
            _userManager = userManager;
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpGet("GetAllOffers")]
        public async Task<ActionResult<IEnumerable<GetAllOffers>>> GetAllOffers()
        {
            if (ModelState.IsValid)
            {
                var data = await _offerRepository.ListAllAsync();

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpGet("GetOfferDetails/{id}")]
        public async Task<IActionResult> GetOfferDetails(int id)
        {
            if (ModelState.IsValid)
            {
                var data = await _offerRepository.GetByIdAsync(id);

                if (data == null)
                {
                    return BadRequest($"Not Found : {id}");
                }
                else
                    return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpPost("CreateOffer")]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOffer offer)
        {
            if (ModelState.IsValid)
            {
                var data = await _offerRepository.AddAsync(offer);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpPut("UpdateOfffer/{id}")]
        public async Task<IActionResult> UpdateOfffer(int id, [FromBody] UpdateOfffer offer)
        {
            offer.Id = id; // Ensure Id matches update target

            try
            {
                var updatedOffer = await _offerRepository.UpdateAsync(offer);

                return Ok(updatedOffer); 
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Handle offer not found gracefully
            }
        }


        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpDelete("DeleteOffer/{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            if (ModelState.IsValid)
            {
                var data = await _offerRepository.DeleteAsync(id);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpGet("GetAllTrainees")]
        public async Task<IActionResult> GetAllTrainees()
        {
            var userRole = await _userManager.GetUsersInRoleAsync(clsRoles.roleUser);
            if (userRole == null)
            {
                return NotFound($"No users found in role {clsRoles.roleUser}.");
            }

            var users = userRole.Select(user => new
            {
                Id = user.Id,
                Username = user.UserName
            }).ToList();

            return Ok(users);
        }
    }
}
