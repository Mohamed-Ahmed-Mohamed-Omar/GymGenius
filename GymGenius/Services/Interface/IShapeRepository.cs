using GymGenius.Models;
using GymGenius.Models.Shape;

namespace GymGenius.Services.Interface
{
    public interface IShapeRepository
    {
        Task<IEnumerable<GetAllShapes>> ListAllAsync(int FromId, int CurrentFoalId);

        Task<ResponseGeneral> AddAsync(CreateShape entity);

        Task<ResponseGeneral> DeleteAsync(int id);
    }
}
