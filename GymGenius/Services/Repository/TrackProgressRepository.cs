using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.TrackProgresses;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GymGenius.Services.Repository
{
    public class TrackProgressRepository : ITrackProgressRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TrackProgressRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreateTrackProgress entity, string UserName)
        {
            var RG = new ResponseGeneral();

            try
            {

                Track_Progress data = new()
                {
                    Level = entity.Level,
                    Score = entity.Score,
                    UserName = UserName
                };

                data.Time = DateTime.Now;

                await _context.track_Progresses.AddAsync(data);

                RG.Done = true;

                RG.Message = "Done";
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                RG.Done = false;

                RG.Message = $"An error occurred: {ex.Message}";
            }

            return RG;
        }

        public async Task<IEnumerable<int>> GeTAllLevelAsync(string UserName)
        {
            var levels = await _context.track_Progresses
                .Where(r => r.UserName == UserName)
                .Select(s => s.Level)
                .ToListAsync();

            return levels;
        }

        public async Task<IEnumerable<GetAllTrackProgress>> GetAllTrackProgressesAsync(string UserName)
        {
            var data = await _context.track_Progresses.Where(r => r.UserName == UserName)
                .Select(s => new GetAllTrackProgress
                {
                    Level = s.Level,
                    Score = s.Score,
                    UploadDate = s.Time
                }).ToListAsync();

            return data;
        }
    }
}
