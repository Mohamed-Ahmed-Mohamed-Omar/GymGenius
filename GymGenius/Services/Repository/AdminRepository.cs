using GymGenius.Data;
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
        private readonly ApplicationDbContext _context;

        public AdminRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ResponseGeneral> AddUserToRoleAsync(string UserNameOrID, string RoleName)
        {
            var RG = new ResponseGeneral();

            // Check if the user exists by username
            var user = await _userManager.FindByNameAsync(UserNameOrID);
            if (user == null)
            {
                // If not found by username, check by ID
                user = await _userManager.FindByIdAsync(UserNameOrID);

                if (user == null)
                {
                    RG.Message = $"User with UserNameOrID: '{UserNameOrID}' not found.";
                    RG.Done = false;
                    return RG;
                }
            }

            // Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(RoleName);
            if (!roleExists)
            {
                RG.Message = $"Role '{RoleName}' not found.";
                RG.Done = false;
                return RG;
            }

            // Check if the user is already in the role
            if (await _userManager.IsInRoleAsync(user, RoleName))
            {
                RG.Message = $"User {UserNameOrID} is already in role {RoleName}.";
                RG.Done = false;
                return RG;
            }

            // Add the user to the role
            var result = await _userManager.AddToRoleAsync(user, RoleName);

            if (result.Succeeded)
            {
                RG.Message = $"User {UserNameOrID} added to role {RoleName} successfully.";
                RG.Done = true;
            }
            else
            {
                RG.Message = $"Failed to add user {UserNameOrID} to role {RoleName}.";
                RG.Done = false;
            }

            return RG;
        }

        public async Task<IEnumerable<object>> GetAllRolesAsync()
        {
            var data = await _roleManager.Roles.Select(role => new
            {
                id = role.Id,
                name = role.Name
            }).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<object>> GetAllUserByRoleNameAsync(string RoleName)
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
                id = user.Id,
                username = user.UserName
            }).ToList();

            return users;
        }

        public async Task<ResponseGeneral> RemoveUserFromRoleAsync(string UserNameOrID, string RoleName)
        {
            var RG = new ResponseGeneral();

            // Check if the user exists by username
            var user = await _userManager.FindByNameAsync(UserNameOrID);
            if (user == null)
            {
                // If not found by username, check by ID
                user = await _userManager.FindByIdAsync(UserNameOrID);

                if (user == null)
                {
                    RG.Message = $"User with UserNameOrID: '{UserNameOrID}' not found.";
                    RG.Done = false;
                    return RG;
                }
            }

            // Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(RoleName);
            if (!roleExists)
            {
                RG.Message = $"Role '{RoleName}' not found.";
                RG.Done = false;
                return RG;
            }

            // Check if the user is in the role
            if (!await _userManager.IsInRoleAsync(user, RoleName))
            {
                RG.Message = $"User {UserNameOrID} is not in role {RoleName}.";
                RG.Done = false;
                return RG;
            }

            // Remove the user from the role
            var result = await _userManager.RemoveFromRoleAsync(user, RoleName);

            if (result.Succeeded)
            {
                RG.Message = $"User {UserNameOrID} removed from role {RoleName} successfully.";
                RG.Done = true;
            }
            else
            {
                RG.Message = $"Failed to remove user {UserNameOrID} from role {RoleName}.";
                RG.Done = false;
            }

            return RG;
        }

        public async Task<ResponseGeneral> RemoveUserAsync(string UserNameO)
        {
            var response = new ResponseGeneral();

            // Check if the user exists
            var user = await _userManager.FindByNameAsync(UserNameO);
            if (user == null)
            {
                response.Message = $"User with username '{UserNameO}' not found.";
                response.Done = false;
                return response;
            }

            // Optional: Add additional logic to handle related entities if necessary
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Fetch and delete related entities manually if necessary
                    var subscriptions = _context.subscriptions.Where(s => s.UserName == UserNameO);
                    _context.subscriptions.RemoveRange(subscriptions);

                    var TrackProgress = _context.track_Progresses.Where(t => t.UserName == UserNameO);
                    _context.track_Progresses.RemoveRange(TrackProgress);

                    var Product = _context.products.Where(p => p.UserName == UserNameO);
                    _context.products.RemoveRange(Product);

                    await _context.SaveChangesAsync();

                    // Delete the user
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        response.Message = $"User {UserNameO} deleted successfully.";
                        response.Done = true;
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        // Handle errors if deletion fails
                        response.Message = $"Failed to delete user {UserNameO}.";
                        response.Done = false;
                        foreach (var error in result.Errors)
                        {
                            response.Message += $" {error.Description}";
                        }
                        await transaction.RollbackAsync();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    response.Message = $"Exception occurred: {ex.Message}";
                    response.Done = false;
                    await transaction.RollbackAsync();
                }
            }

            return response;
        }
    }
}
