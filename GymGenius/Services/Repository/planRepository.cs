using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Plans;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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

        public async Task<IEnumerable<GetAllPlans>> GetAllPlansAsync()
        {
            var data = await _context.plans.Select(m => new GetAllPlans
            {
                Id = m.Id,
                Name = m.Name,
                Place = m.Training_From.Name,
                Goal = m.Current_Goal.Name,
                price = m.Price,
                Num_of_Quotas = m.Num_of_Quotas,
                CountRegister = _context.subscriptions.Count(sub => sub.PlanId == m.Id)
            }).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<GetAllPlansByPlaceandGoal>> GetAllPlansByPlaceandGoalAsync(int PlaceId, int GoalId)
        {
            var data = await _context.plans.Where(d => d.Training_FromId == PlaceId && d.Current_GoalId == GoalId)
                .Select(m => new GetAllPlansByPlaceandGoal
                {
                    Num_of_Quotas = m.Num_of_Quotas,
                    Name = m.Name,
                    Id = m.Id,
                    price = m.Price
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
