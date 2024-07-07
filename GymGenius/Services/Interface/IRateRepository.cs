using GymGenius.Models;
using GymGenius.Models.Rates;

namespace GymGenius.Services.Interface
{
    public interface IRateRepository
    {
        Task<ResponseGeneral> AddAsync(CreateRate createRate, string UserName);

        Task<IEnumerable<GetAllRates>> GetAllAsync(string UserName);

        Task<float> GetRateAsync(string UserName);
    }

}