using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Advertisements;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Services.Repository
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AdvertisementRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreateAdvertisement entity)
        {
            var RG = new ResponseGeneral();

            try 
            {
                var data = _mapper.Map<Advertisement>(entity);

                await _context.advertisements.AddAsync(data);

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
            var delData = await _context.advertisements.FindAsync(id);

            if (delData != null)
            {
                _context.advertisements.Remove(delData);

                await _context.SaveChangesAsync();

                return new ResponseGeneral { Done = true, Message = "Successful" };
            }

            return new ResponseGeneral { Message = "Not Found" };
        }

        public async Task<GetAdvertisementDetails> GetByIdAsync(int id)
        {
            var data = await _context.advertisements.FindAsync(id);

            var response = _mapper.Map<GetAdvertisementDetails>(data);

            return response;
        }

        public async Task<IEnumerable<GetAllAdvertisements>> ListAllAsync()
        {
            var advertisements = await _context.advertisements.ToListAsync();

            var mappedadvertisements = _mapper.Map<IEnumerable<GetAllAdvertisements>>(advertisements);

            return mappedadvertisements;
        }

        public async Task<ResponseGeneral> UpdateAsync(UpdateAdvertisement entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var model = await _context.advertisements.FindAsync(entity.Id);

                if(model == null)
                {
                    throw new ArgumentException("Advertisement not found with the provided Id.");
                }

                _mapper.Map(entity, model);

                _context.advertisements.Update(model);

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
