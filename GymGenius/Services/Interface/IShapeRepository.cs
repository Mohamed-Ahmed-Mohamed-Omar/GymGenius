using GymGenius.Models;
using GymGenius.Models.Shape;
using GymGenius.Models.Shapes;

namespace GymGenius.Services.Interface
{
    public interface IShapeRepository
    {
        Task<IEnumerable<GetAllShapes>> ListAllAsync(int FromId, int CurrentFoalId, int TargetId);

        Task<IEnumerable<GetAllNameTraining>> ListAllAsync();

        Task<ResponseGeneral> AddAsync(CreateShape entity);

        Task<ResponseGeneral> DeleteAsync(int id);
    }
}
