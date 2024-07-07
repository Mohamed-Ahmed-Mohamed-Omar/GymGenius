using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Shape;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Services.Repository
{
    public class ShapeRepository : IShapeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ShapeRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreateShape entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var data = _mapper.Map<Shape_Training>(entity);

                await _context.shape_Trainings.AddAsync(data);

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
            var delData = await _context.shape_Trainings.FindAsync(id);

            if (delData != null)
            {
                _context.shape_Trainings.Remove(delData);

                await _context.SaveChangesAsync();

                return new ResponseGeneral { Done = true, Message = "Successful" };
            }

            return new ResponseGeneral { Message = "Not Found" };
        }

        public async Task<IEnumerable<GetAllShapes>> ListAllAsync(string UserName, int NumberDay)
        {
            var data = await _context.shape_Trainings.Where(d => d.Training_FromId == GetFromId(UserName) && d.Current_GoalId == GetGoalId(UserName)
            && d.SubGoalId == GetSubGoalId(UserName) && d.DayNumber == NumberDay && d.LevelId == GetLevelId(UserName))
                .Select(m => new GetAllShapes
                {
                    TrainName = m.TrainName,
                    ExerciseDescription = m.ExerciseDescription,
                    ExerciseDuration = m.ExerciseDuration,
                    FocusZone = m.FocusZone,
                    ImageName = m.ImageName,
                    LevelName = m.Level_Train.LevelName,
                    NumberOfReps = m.NumberOfReps,
                    TargetMuscle = m.TargetMuscle,
                    VideoName = m.VideoName,
                    countDay = m.DayNumber
                }).ToListAsync();

            return data;
        }

        #region Helpers 
        private int GetLevelId(string UserName)
        {
            var data = _context.subscriptions.Where(s => s.UserName == UserName)
                .Select(s => s.Level_TrainId).FirstOrDefault();

            return data;
        }

        private int GetFromId(string UserName)
        {
            var data = _context.subscriptions.Where(s => s.UserName == UserName)
                .Select(s => s.Training_FromId).FirstOrDefault();

            return data;
        }

        private int GetGoalId(string UserName)
        {
            var data = _context.subscriptions.Where(s => s.UserName == UserName)
                .Select(s => s.Current_GoalId).FirstOrDefault();

            return data;
        }

        private int GetSubGoalId(string UserName)
        {
            var data = _context.subscriptions.Where(s => s.UserName == UserName)
                .Select(s => s.SubGoalId).FirstOrDefault();

            return data;
        }

        #endregion
    }
}
