using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Plans;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Services.Repository
{
    public class planRepository : IplanRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public planRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreatePlan entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var data = _mapper.Map<Plan>(entity);

                await _context.plans.AddAsync(data);

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

        public async Task<ResponseGeneral> DeleteAsync(int id)
        {
            var delData = await _context.plans.FindAsync(id);

            if (delData != null)
            {
                _context.plans.Remove(delData);

                await _context.SaveChangesAsync();

                return new ResponseGeneral { Done = true, Message = "Successful" };
            }

            return new ResponseGeneral { Message = "Not Found" };
        }

        public async Task<IEnumerable<GetPlanDetails>> GetByIdAsync(int LevelId, int Ex_TimeId, int Training_FromId, int Current_GoalId, int Target_MuscleId)
        {
            var data = await _context.plans.Where(d => d.LevelId == LevelId && d.Ex_TimeId == Ex_TimeId &&
            d.Training_FromId == Training_FromId && d.Current_GoalId == Current_GoalId && d.Target_MuscleId == Target_MuscleId)
                .Select(m => new GetPlanDetails
                {
                    Description = m.Description,
                    Name = m.Name,
                    Id = m.Id
                }).ToListAsync();

            return data;
        }

        public async Task<ResponseGeneral> UpdateAsync(UpdatePlan entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var existingOffer = await _context.plans.FindAsync(entity.Id); // Handle cases where Id is not set

                if (existingOffer == null)
                {
                    throw new ArgumentException("Plan not found with the provided Id.");
                }

                _mapper.Map(entity, existingOffer);

                _context.plans.Update(existingOffer);

                await _context.SaveChangesAsync();

                RG.Done = true;

                RG.Message = "Done";
            }
            catch (Exception ex)
            {
                RG.Done = false;

                RG.Message = $"An error occurred: {ex.Message}";
            }

            return RG;
        }
    }
}
