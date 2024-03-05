using GymGenius.Models.Advertisements;
using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymGenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementRepository _advertisementRepository;

        public AdvertisementsController(IAdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }


        [Authorize]
        [HttpGet("GetAllAdverts")]
        public async Task<ActionResult<IEnumerable<GetAllAdvertisements>>> GetAllAdverts()
        {
            if (ModelState.IsValid)
            {
                var data = await _advertisementRepository.ListAllAsync();

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize]
        [HttpGet("GetAdvertDetails/{id}")]
        public async Task<IActionResult> GetAdvertDetails(int id)
        {
            if (ModelState.IsValid)
            {
                var data = await _advertisementRepository.GetByIdAsync(id);

                if (data == null)
                {
                    return BadRequest($"Not Found : {id}");
                }
                else
                    return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleAdmin)]
        [HttpPost("CreateAdvert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdvert([FromBody] CreateAdvertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                var data = await _advertisementRepository.AddAsync(advertisement);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = clsRoles.roleAdmin)]
        [HttpPut("UpdateAdvert/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdvert(int id, [FromBody] UpdateAdvertisement advertisement)
        {
            advertisement.Id = id; // Ensure Id matches update target

            try
            {
                var updatedAdvertisement = await _advertisementRepository.UpdateAsync(advertisement);

                return Ok(updatedAdvertisement);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Handle advertisement not found gracefully
            }
        }


        [Authorize(Roles = clsRoles.roleAdmin)]
        [HttpDelete("DeleteAdvert/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdvert(int id)
        {
            if (ModelState.IsValid)
            {
                var data = await _advertisementRepository.DeleteAsync(id);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }
    }
}
