using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Subscriptions;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Services.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SubscriptionRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreateSubscription entity, string UserName, int Age)
        {
            var RG = new ResponseGeneral();

            try
            {
                Subscription data = new()
                {
                    Age = Age,
                    UserName = UserName,
                    Current_GoalId = entity.Current_GoalId,
                    Fat = entity.Fat,
                    Height = entity.Height,
                    Level_TrainId = entity.Level_TrainId,
                    Target_MuscleId = entity.Target_MuscleId,
                    TimeId = entity.TimeId,
                    Training_FromId = entity.Training_FromId,
                    Weight = entity.Weight,
                    SubscriptionDays = entity.Days.Select(d => new SubscriptionDay { DayId = d }).ToList()
                };

                await _context.subscriptions.AddAsync(data);

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
            var delData = await _context.subscriptions.FindAsync(id);

            if (delData != null)
            {
                _context.subscriptions.Remove(delData);

                await _context.SaveChangesAsync();

                return new ResponseGeneral { Done = true, Message = "Successful" };
            }

            return new ResponseGeneral { Message = "Not Found" };
        }

        public async Task<ResponseGeneral> DeleteAsync(string UserName)
        {
            var delData = await _context.subscriptions.FindAsync(UserName);

            if (delData != null)
            {
                _context.subscriptions.Remove(delData);

                await _context.SaveChangesAsync();

                return new ResponseGeneral { Done = true, Message = "Successful" };
            }

            return new ResponseGeneral { Message = "Not Found" };
        }

        public async Task<IEnumerable<GetAllSubscriptions>> ListAllAsync()
        {
            var subscriptions = await _context.subscriptions.ToListAsync();

            var mappedsubscriptions = _mapper.Map<IEnumerable<GetAllSubscriptions>>(subscriptions);

            return mappedsubscriptions;
        }

        public async Task<GetSubscriptionDetails> GetByIdAsync(int id)
        {
            var data = await _context.subscriptions.Select(d => new GetSubscriptionDetails
            {
                Id = d.Id,
                Age = d.Age,
                Fat = d.Fat,
                Height = d.Height,
                Level_Train = d.Level_Train.LevelName,
                Current_Goal = d.Current_Goal.Name,
                Time = d.Time.Name,
                Weight = d.Weight,
                UserName = d.UserName,
                Training_From = d.Training_From.Name,
                Target_Muscle = d.Target_Muscle.Name
            }).FirstOrDefaultAsync();

            return data;
        }

        public async Task<GetSubscriptionDetails> GetByIdAsync(string UserName)
        {
            var data = await _context.subscriptions.Where(d => d.UserName == UserName)
                .Select(d => new GetSubscriptionDetails
                {
                    Age = d.Age,
                    Fat = d.Fat,
                    Height = d.Height,
                    Level_Train = d.Level_Train.LevelName,
                    Current_Goal = d.Current_Goal.Name,
                    Time = d.Time.Name,
                    Weight = d.Weight,
                    Training_From = d.Training_From.Name,
                    Target_Muscle = d.Target_Muscle.Name
                }).FirstOrDefaultAsync();

            return data;
        }

        public async Task<ResponseGeneral> UpdateAsync(UpdateSubscription entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var existingOffer = await _context.subscriptions.FindAsync(entity.Id); // Handle cases where Id is not set

                if (existingOffer == null)
                {
                    throw new ArgumentException("Subscription not found with the provided Id.");
                }

                _mapper.Map(entity, existingOffer);

                _context.subscriptions.Update(existingOffer);

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

        public async Task<ResponseGeneral> UpdateAsync(UpdateSubscriptionToUser entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var existingOffer = await _context.subscriptions.FindAsync(entity.UserName); // Handle cases where UserName is not set

                if (existingOffer == null)
                {
                    throw new ArgumentException("Subscription not found with the provided UserName.");
                }

                _mapper.Map(entity, existingOffer);

                _context.subscriptions.Update(existingOffer);

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

        public async Task<IEnumerable<SubscriptionDayGetDetauls>> ListAllDaysAsync(int Id)
        {
            var data = await _context.subscriptionsDays.Where(d => d.SubscriptionId == Id)
                .Select(m => new SubscriptionDayGetDetauls
                {
                    DayName = m.Day.Name
                }).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<SubscriptionDayGetDetauls>> ListAllDaysAsync(string UserName)
        {
            var data = await _context.subscriptionsDays.Where(d => d.Subscription.UserName == UserName)
                .Select(m => new SubscriptionDayGetDetauls
                {
                    DayName = m.Day.Name
                }).ToListAsync();

            return data;
        }
    }
}
