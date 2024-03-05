using GymGenius.Models;
using GymGenius.Models.Identity;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace GymGenius.Services.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseGeneral> AddUserToRoleAsync(string UserNameOrID, string RoleName)
        {
            var RG = new ResponseGeneral();

            var user = await _userManager.FindByNameAsync(UserNameOrID);
            if (user == null)
            {
                user = await _userManager.FindByIdAsync(UserNameOrID);

                if (user == null)
                {
                    RG.Message = $"User With UserNameOrID : '{UserNameOrID}' not found.";
                    RG.Done = false;
                }
            }

            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                RG.Message = $"Role {RoleName} not found.";
                RG.Done = false;
            }

            await _userManager.AddToRoleAsync(user, RoleName);

            RG.Message = $"User {UserNameOrID} add to Role {RoleName} successfully.";
            RG.Done = true; 

            return RG;
        }

        public async Task<IQueryable<string>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(role => new
            {
                Id = role.Id,
                Name = role.Name
            }).ToListAsync();

            return (IQueryable<string>)roles;
        }

        public async Task<IEnumerable<string>> GetAllUserByRoleNameAsync(string RoleName)
        {
            var userRole = await _userManager.GetUsersInRoleAsync(RoleName);
            if (userRole == null)
            {
                throw new NotFoundException($"No users found in role {RoleName}.");
            }

            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                throw new BadRequestException($"Role {RoleName} not found.");
            }

            var users = userRole.Select(user => new
            {
                Id = user.Id,
                Username = user.UserName
            }).ToList();

            return (IEnumerable<string>)users;
        }

        public async Task<ResponseGeneral> RemoveserFromRoleAsync(string UserNameOrID, string RoleName)
        {
            var RG = new ResponseGeneral();

            var user = await _userManager.FindByNameAsync(UserNameOrID);
            if (user == null)
            {
                user = await _userManager.FindByIdAsync(UserNameOrID);

                if (user == null)
                {
                    RG.Message = $"User With UserNameOrID : '{UserNameOrID}' not found.";
                    RG.Done = false ;
                }
            }

            if (!await _userManager.IsInRoleAsync(user, RoleName))
            {
                RG.Message = $"User {UserNameOrID} is not in role {RoleName}.";
                RG.Done = false ;
            }

            await _userManager.RemoveFromRoleAsync(user, RoleName);

            RG.Message = $"User {UserNameOrID} Remove to Role {RoleName} successfully.";
            RG.Done = true;

            return RG;
        }

        public async Task<ResponseGeneral> RemoveUserAsync(string UserNameOrID)
        {
            ApplicationUser user = new ApplicationUser();

            var RG = new ResponseGeneral();

            if (UserNameOrID.Contains("@"))
            {
                user = await _userManager.FindByNameAsync(UserNameOrID);

                if (user == null)
                {
                    RG.Message = $"User With User : '{UserNameOrID}' not found.";
                    RG.Done = false;
                }
            }
            else
            {
                user = await _userManager.FindByIdAsync(UserNameOrID);

                if (user == null)
                {
                    RG.Message = $"User With ID : '{UserNameOrID}' not found.";
                    RG.Done = false;
                }
            }

            await _userManager.DeleteAsync(user);

            RG.Message = $"User {UserNameOrID} delete successfully.";
            RG.Done = true;

            return RG;
        }
    }
}
