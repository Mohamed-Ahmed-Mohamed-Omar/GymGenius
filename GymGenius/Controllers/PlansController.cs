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
        [HttpGet("GetAllPlansByLevelAndTimeAndTraining_FromAndCurrent_GoalAndTarget_Muscle/{LevelId}/{Ex_TimeId}/{Training_FromId}/{Current_GoalId}/{Target_MuscleId}")]
        public async Task<ActionResult<IEnumerable<GetPlanDetails>>> GetAllPlansByLevelAndTimeAndTraining_FromAndCurrent_GoalAndTarget_Muscle(int LevelId, int Ex_TimeId, int Training_FromId, int Current_GoalId, int Target_MuscleId)
        {
            try
            {

                var data = await _planRepository.GetByIdAsync(LevelId, Ex_TimeId, Training_FromId, Current_GoalId, Target_MuscleId);

                return Ok(data);

            }
            catch (Exception ex)
            {
                return BadRequest($"Not Found {LevelId} and {Ex_TimeId}");
            }
        }


        [Authorize(Roles = clsRoles.roleCoach)]
        [HttpPost("CreatePlan")]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlan plan)
        {
            var data = await _planRepository.AddAsync(plan);

            return Ok(data);
        }


        [Authorize(Roles = clsRoles.roleCoach)]
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


        [Authorize(Roles = clsRoles.roleCoach)]
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
