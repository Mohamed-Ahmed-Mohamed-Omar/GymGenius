using GymGenius.Models.Shape;
using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymGenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShapesController : ControllerBase
    {
        private readonly IShapeRepository _shapeRepository;

        public ShapesController(IShapeRepository shapeRepository)
        {
            _shapeRepository = shapeRepository; 
        }


        [Authorize(Roles = clsRoles.roleUser + "," + clsRoles.roleCoach)]
        [HttpGet("GetAllShapeTrainingByWherePlaceAndGoal/{FromId}/{CurrentFoalId}")]
        public async Task<ActionResult<IEnumerable<GetAllShapes>>> GetAllShapeTrainingByWherePlaceAndGoal(int FromId, int CurrentFoalId)
        {
            if (ModelState.IsValid)
            {
                var data = await _shapeRepository.ListAllAsync(FromId, CurrentFoalId);

                if (data == null)
                {
                    return BadRequest("Not Found Shape Training!");
                }
                else
                    return Ok(data);
            }

            return BadRequest(ModelState);
        }
        

        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpPost("CreateShape")]
        public async Task<IActionResult> CreateShape([FromBody] CreateShape shape)
        {
            if (ModelState.IsValid)
            {
                var data = await _shapeRepository.AddAsync(shape);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }


   
        

        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpDelete("DeleteShape/{id}")]
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
