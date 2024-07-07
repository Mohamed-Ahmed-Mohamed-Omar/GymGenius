using GymGenius.Models;
using GymGenius.Models.Advertisements;

namespace GymGenius.Services.Interface
{
    public interface IAdvertisementRepository
    {
        Task<IEnumerable<GetAllAdvertisements>> ListAllAsync();

        Task<ResponseGeneral> AddAsync(CreateAdvertisement entity);

        Task<ResponseGeneral> UpdateAsync(UpdateAdvertisement entity);

        Task<ResponseGeneral> DeleteAsync(int id);
    }
}
