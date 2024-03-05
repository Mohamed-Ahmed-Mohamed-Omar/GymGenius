using GymGenius.Models;
using GymGenius.Models.Subscriptions;

namespace GymGenius.Services.Interface
{
    public interface ISubscriptionRepository
    {
        Task<ResponseGeneral> AddAsync(CreateSubscription entity, string UserName, int Age);

        Task<ResponseGeneral> DeleteAsync(int id);

        Task<ResponseGeneral> DeleteAsync(string UserName);

        Task<IEnumerable<GetAllSubscriptions>> ListAllAsync();

        Task<GetSubscriptionDetails> GetByIdAsync(int id);

        Task<GetSubscriptionDetails> GetByIdAsync(string UserName);

        Task<ResponseGeneral> UpdateAsync(UpdateSubscription entity);

        Task<ResponseGeneral> UpdateAsync(UpdateSubscriptionToUser entity);

        Task<IEnumerable<SubscriptionDayGetDetauls>> ListAllDaysAsync(int Id);

        Task<IEnumerable<SubscriptionDayGetDetauls>> ListAllDaysAsync(string UserName);
    }
}
