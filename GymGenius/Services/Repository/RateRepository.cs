using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Rates;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Services.Repository
{
    public class RateRepository : IRateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RateRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreateRate createRate, string UserName)
        {
            var RG = new ResponseGeneral();

            try
            {
                Rate data = new Rate()
                {
                    Rating = createRate.Rate,
                    CoachName = GetCoachName(UserName)
                };

                await _context.rates.AddAsync(data);

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

        public async Task<IEnumerable<GetAllRates>> GetAllAsync(string UserName)
        {
            var data = await _context.rates.Where(r => r.CoachName == UserName)
                .Select(s => new GetAllRates
                {
                    Rate = s.Rating
                }).ToListAsync();

            return data;
        }

        public async Task<float> GetRateAsync(string UserName)
        {
            var averageRate = await _context.rates
               .Where(r => r.CoachName == UserName)
               .AverageAsync(r => r.Rating);

            return (float)averageRate;
        }

        private string GetCoachName(string UserName)
        {
            var data =  _context.subscriptions.Where(s=>s.UserName == UserName)
                .Select(s => s.Name_Coach).FirstOrDefault();

            return data;
        }
    }
}
