using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Models;
using GymGenius.Models.Offers;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Services.Repository
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OfferRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreateOffer entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var data = _mapper.Map<Offer>(entity);

                await _context.offers.AddAsync(data);

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
            var delData = await _context.offers.FindAsync(id);

            if (delData != null)
            {
                _context.offers.Remove(delData);

                await _context.SaveChangesAsync();

                return new ResponseGeneral { Done = true, Message = "Successful" };
            }

            return new ResponseGeneral { Message = "Not Found" };
        }

        public async Task<GetOfferDerails> GetByIdAsync(int id)
        {
            var data = await _context.offers.FindAsync(id);

            var response = _mapper.Map<GetOfferDerails>(data);

            return response;
        }

        public async Task<IEnumerable<GetAllOffers>> ListAllAsync()
        {
            var offers = await _context.offers.ToListAsync();

            var mappedOffers = _mapper.Map<IEnumerable<GetAllOffers>>(offers);

            return mappedOffers;
        }

        public async Task<ResponseGeneral> UpdateAsync(UpdateOfffer entity)
        {
            var RG = new ResponseGeneral();

            try
            {
                var existingOffer = await _context.offers.FindAsync(entity.Id); // Handle cases where Id is not set

                if (existingOffer == null)
                {
                    throw new ArgumentException("Offer not found with the provided Id.");
                }

                _mapper.Map(entity, existingOffer);

                _context.offers.Update(existingOffer);

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