using GymGenius.Models;
using GymGenius.Models.TrackProgresses;

namespace GymGenius.Services.Interface
{
    public interface ITrackProgressRepository
    {
        Task<ResponseGeneral> AddAsync(CreateTrackProgress entity, string UserName);

        Task<IEnumerable<int>> GeTAllLevelAsync(string UserName);

        Task<IEnumerable<GetAllTrackProgress>> GetAllTrackProgressesAsync(string UserName);
    }
}
