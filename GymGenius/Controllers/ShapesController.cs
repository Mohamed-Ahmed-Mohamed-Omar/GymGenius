using GymGenius.Models.Identity;
using GymGenius.Models.Shape;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymGenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShapesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IShapeRepository _shapeRepository;

        public ShapesController(IShapeRepository shapeRepository, UserManager<ApplicationUser> userManager)
        {
            _shapeRepository = shapeRepository;
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet("GetTrainingTableToday/{DayNumber}")]
        public async Task<ActionResult<IEnumerable<GetAllShapes>>> GetAllShapeTrainingByWherePlaceAndGoal(int DayNumber)
        {
            if (ModelState.IsValid)
            {
                var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByNameAsync(username);

                var data = await _shapeRepository.ListAllAsync(user.UserName, DayNumber);

                if (data == null)
                {
                    return BadRequest("Not Found Shape Training!");
                }
                else
                    return Ok(data);
            }

            return BadRequest(ModelState);
        }
        

        [Authorize]
        [HttpPost("CreateTrainTable")]
        public async Task<IActionResult> CreateShape([FromBody] CreateShape shape)
        {
            if (ModelState.IsValid)
            {
                var data = await _shapeRepository.AddAsync(shape);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }
      

        [Authorize]
        [HttpDelete("DeleteTrainTable/{id}")]
        public async Task<IActionResult> DeleteShape(int id)
        {
            if (ModelState.IsValid)
            {
                var data = await _shapeRepository.DeleteAsync(id);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }

    }
}
