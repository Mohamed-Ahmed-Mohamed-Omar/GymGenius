using GymGenius.Models;
using GymGenius.Models.Subscriptions;

namespace GymGenius.Services.Interface
{
    public interface ISubscriptionRepository
    {
        Task<ResponseGeneral> AddAsync(CreateSubscription entity, string UserName, int Age);

        Task<ResponseGeneral> DeleteAsync(int id);

        Task<IEnumerable<GetSubscriptionDetails>> ListAllAsync();

        Task<GetSubscriptionDetails> GetByIdAsync(string UserName);

        Task<ResponseGeneral> UpdateAsync(UpdateSubscription entity);

        Task<ResponseGeneral> UpdateAsync(UpdateSubscriptionToUser entity);

        Task<IEnumerable<object>> GetAllTrainnes(string NameCoach);

        Task<IEnumerable<object>> GetAllCoaches(string UserName);

        Task<bool> IsAvailable(string UserName);
    }
}
