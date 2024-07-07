using GymGenius.Models;
using GymGenius.Models.Shape;

namespace GymGenius.Services.Interface
{
    public interface IShapeRepository
    {
        Task<IEnumerable<GetAllShapes>> ListAllAsync(string UserName, int NumberDay);

        Task<ResponseGeneral> AddAsync(CreateShape entity);

        Task<ResponseGeneral> DeleteAsync(int id);
    }
}
