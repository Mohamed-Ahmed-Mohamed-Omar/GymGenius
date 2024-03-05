using GymGenius.Models;
using GymGenius.Models.Offers;

namespace GymGenius.Services.Interface
{
    public interface IOfferRepository
    {
        Task<GetOfferDerails> GetByIdAsync(int id);

        Task<IEnumerable<GetAllOffers>> ListAllAsync();

        Task<ResponseGeneral> AddAsync(CreateOffer entity);

        Task<ResponseGeneral> UpdateAsync(UpdateOfffer entity);

        Task<ResponseGeneral> DeleteAsync(int id);
    }
}
