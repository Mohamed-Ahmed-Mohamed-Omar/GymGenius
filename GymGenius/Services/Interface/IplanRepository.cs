using GymGenius.Models;
using GymGenius.Models.Plans;

namespace GymGenius.Services.Interface
{
    public interface IplanRepository
    {
        Task<IEnumerable<GetPlanDetails>> GetByIdAsync(int LevelId, int Ex_TimeId, int Training_FromId, int Current_GoalId, int Target_MuscleId);

        Task<ResponseGeneral> AddAsync(CreatePlan entity);

        Task<ResponseGeneral> UpdateAsync(UpdatePlan entity);

        Task<ResponseGeneral> DeleteAsync(int id);
    }
}
