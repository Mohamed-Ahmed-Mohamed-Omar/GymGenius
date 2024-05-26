using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;

namespace GymGenius.Controllers
{
    [Authorize(Roles = clsRoles.roleAdmin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminsController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpPost("AddUserToRole/{UserNameOrID}/{RoleName}")]
        public async Task<IActionResult> AddUserToRole(string UserNameOrID, string RoleName)
        {
            var data = await _adminRepository.AddUserToRoleAsync(UserNameOrID, RoleName);

            return Ok(data);
        }


        [HttpDelete("RemoveUserFromRole/{UserNameOrID}/{RoleName}")]
        public async Task<IActionResult> RemoveUserFromRoleAsync(string UserNameOrID, string RoleName)
        {
            var data = await _adminRepository.RemoveUserFromRoleAsync(UserNameOrID, RoleName);

            return Ok(data);
        }


        [HttpGet("GetAllUserByRoleName/{RoleName}")]
        public async Task<IActionResult> GetAllUserByRoleName(string RoleName)
        {
            try
            {
                var users = await _adminRepository.GetAllUserByRoleNameAsync(RoleName);
                return Ok(users);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("RemoveUser/{UserNameOrID}")]
        public async Task<IActionResult> RemoveUser(string UserNameOrID)
        {
            var data = await _adminRepository.RemoveUserAsync(UserNameOrID);

            if (data == null) return BadRequest("Not Found");

            return Ok(data);
        }


        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _adminRepository.GetAllRolesAsync();

            return Ok(roles);
        }
    }
}
