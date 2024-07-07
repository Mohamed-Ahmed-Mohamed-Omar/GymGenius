using GymGenius.Models;
using GymGenius.Models.Plans;

namespace GymGenius.Services.Interface
{
    public interface IplanRepository
    {
        Task<IEnumerable<GetAllPlansByPlaceandGoal>> GetAllPlansByPlaceandGoalAsync(int PlaceId, int GoalId);

        Task<IEnumerable<GetAllPlans>> GetAllPlansAsync();

        Task<ResponseGeneral> AddAsync(CreatePlan entity);

        Task<ResponseGeneral> UpdateAsync(UpdatePlan entity);

        Task<ResponseGeneral> DeleteAsync(int id);
    }
}
