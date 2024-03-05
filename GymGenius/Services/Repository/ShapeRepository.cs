using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Shape;
using GymGenius.Models.Shapes;
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

        public async Task<IEnumerable<GetAllShapes>> ListAllAsync(int FromId, int CurrentFoalId, int TargetId)
        {
            var data = await _context.shape_Trainings.Where(d => d.Training_FromId == FromId && d.Current_GoalId == CurrentFoalId
                     && d.Target_MuscleId == TargetId).Select(m=> new GetAllShapes
                     {
                         Id = m.Id,
                         ImageName = m.ImageName,
                         VideoName = m.VideoName
                     }).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<GetAllNameTraining>> ListAllAsync()
        {
            var shapes = await _context.shape_Trainings.ToListAsync();

            var mappedShapes = _mapper.Map<IEnumerable<GetAllNameTraining>>(shapes);

            return mappedShapes;
        }
    }   
}
