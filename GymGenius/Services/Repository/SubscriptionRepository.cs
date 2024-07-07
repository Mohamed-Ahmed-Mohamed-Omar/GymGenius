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
                    Training_FromId = entity.Training_FromId,
                    Weight = entity.Weight,
                    Rest = entity.Rest,
                    Name_Coach = entity.Name_Coach,
                    Subscription_Duration = entity.Subscription_Duration,
                    Subscription_Status = entity.Subscription_Status,
                    Time_End = entity.Time_End,
                    Time_Start = DateTime.Now,
                    GenderId = entity.GenderId,
                    PlanId = entity.PlanId,
                    SubGoalId = entity.SubGoalId
                };

                DayTrainNumber day = new()
                {
                    UserName = UserName,
                };

                await _context.dayTrainNumbers.AddAsync(day);

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

        public async Task<IEnumerable<GetSubscriptionDetails>> ListAllAsync()
        {
            var subscriptions = await _context.subscriptions.Select(d => new GetSubscriptionDetails
            {
                Id = d.Id,
                Age = d.Age,
                Fat = d.Fat,
                Height = d.Height,
                Level_Train = d.Level_Train.LevelName,
                Goal = d.Current_Goal.Name,
                Weight = d.Weight,
                UserName = d.UserName,
                Place = d.Training_From.Name,
                Name_Coach = d.Name_Coach,
                Rest = d.Rest,
                Subscription_Duration = d.Subscription_Duration,
                Subscription_Status = d.Subscription_Status,
                Time_End = d.Time_End,
                Time_Start = d.Time_Start,
                Gender = d.Gender.Gender_Type,
                PlanName = d.Plan.Name,
                SubGoalName = d.SubGoal.Name
            }).ToListAsync();

            return subscriptions;
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
                    Goal = d.Current_Goal.Name,
                    Weight = d.Weight,
                    Place = d.Training_From.Name,
                    Name_Coach = d.Name_Coach,
                    Rest = d.Rest,
                    Subscription_Duration = d.Subscription_Duration,
                    Subscription_Status = d.Subscription_Status,
                    Time_End = d.Time_End,
                    Time_Start = d.Time_Start,
                    Gender = d.Gender.Gender_Type,
                    PlanName = d.Plan.Name,
                    SubGoalName = d.SubGoal.Name
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

        public async Task<IEnumerable<object>> GetAllTrainnes(string NameCoach)
        {
            var data = await _context.subscriptions.Where(d => d.Name_Coach == NameCoach)
                .Select(d => new
                {
                    Id = d.Id,
                    Age = d.Age,
                    Fat = d.Fat,
                    Height = d.Height,
                    Level_Train = d.Level_Train.LevelName,
                    Goal = d.Current_Goal.Name,
                    Weight = d.Weight,
                    UserName = d.UserName,
                    Place = d.Training_From.Name,
                    Rest = d.Rest,
                    Subscription_Duration = d.Subscription_Duration,
                    Subscription_Status = d.Subscription_Status,
                    Time_End = d.Time_End,
                    Time_Start = d.Time_Start,
                    Gender = d.Gender.Gender_Type,
                    PlanName = d.Plan.Name,
                    SubGoalName = d.SubGoal.Name
                }).ToArrayAsync();

            return data;
        }

        public async Task<IEnumerable<object>> GetAllCoaches(string UserName)
        {
            var data = await _context.subscriptions.GroupBy(d => d.Name_Coach == UserName)
                .Select(d => new
                {
                    TraineeCount = d.Count(),
                    CoachName = d.Key
                }).ToArrayAsync();

            return data;
        }

        public async Task<bool> IsAvailable(string UserName)
        {
            // Check if a subscription with the given username already exists
            bool userExists = await _context.subscriptions.AnyAsync(s => s.UserName == UserName);

            if (userExists == true) return true;
            else return false;
        }
    }
}
