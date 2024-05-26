using GymGenius.Models.Plans;
using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymGenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly IplanRepository _planRepository;

        public PlansController(IplanRepository planRepository)
        {
            _planRepository = planRepository;
        }


        [Authorize(Roles = clsRoles.roleUser)]
        [HttpGet("GetAllPlansByPlaceAndGoal/{PlaceId}/{GoalId}")]
        public async Task<ActionResult<IEnumerable<GetAllPlansByPlaceandGoal>>> GetAllPlansByPlaceAndGoal(int PlaceId, int GoalId)
        {
            try
            {

                var data = await _planRepository.GetAllPlansByPlaceandGoalAsync(PlaceId, GoalId);

                return Ok(data);

            }
            catch (Exception ex)
            {
                return BadRequest($"Not Found {PlaceId} and {GoalId}");
            }
        }


        [Authorize(Roles = clsRoles.roleCoach + "," + clsRoles.roleAdmin)]
        [HttpGet("GetAllPlans")]
        public async Task<ActionResult<IEnumerable<GetAllPlans>>> GetAllPlans()
        {
            try
            {
                var data = await _planRepository.GetAllPlansAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("Not Found");
            }
        }


        [Authorize(Roles = clsRoles.roleCoach + "," + clsRoles.roleAdmin)]
        [HttpPost("CreatePlan")]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlan plan)
        {
            var data = await _planRepository.AddAsync(plan);

            return Ok(data);
        }


        [Authorize(Roles = clsRoles.roleCoach + "," + clsRoles.roleAdmin)]
        [HttpPut("UpdatePlan/{id}")]
        public async Task<IActionResult> UpdatePlan(int id, [FromBody] UpdatePlan plan)
        {
            plan.Id = id; // Ensure Id matches update target

            try
            {
                var updatedPlan = await _planRepository.UpdateAsync(plan);

                return Ok(updatedPlan);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Handle offer not found gracefully
            }
        }


        [Authorize(Roles = clsRoles.roleCoach + "," + clsRoles.roleAdmin)]
        [HttpDelete("DeletePlan/{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            if (ModelState.IsValid)
            {
                var data = await _planRepository.DeleteAsync(id);

                return Ok(data);
            }

            return BadRequest(ModelState);
        }
    }
}
