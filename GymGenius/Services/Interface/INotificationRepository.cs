using GymGenius.Models;
using GymGenius.Models.Notifications;

namespace GymGenius.Services.Interface
{
    public interface INotificationRepository
    {
        Task<IEnumerable<GetAllNotifications>> ListAllAsync();

        Task<ResponseGeneral> AddAsync(CreateNotification entity);

        Task<ResponseGeneral> DeleteAsync(int id);
    }
}
