using GymGenius.Models;

namespace GymGenius.Services.Interface
{
    public interface IAdminRepository
    {
        Task<ResponseGeneral> AddUserToRoleAsync(string UserNameOrID, string RoleName);

        Task<ResponseGeneral> RemoveUserFromRoleAsync(string UserNameOrID, string RoleName);

        Task<IEnumerable<object>> GetAllUserByRoleNameAsync(string RoleName);

        Task<ResponseGeneral> RemoveUserAsync(string UserNameOrID);

        Task<IEnumerable<object>> GetAllRolesAsync();
    }
}
