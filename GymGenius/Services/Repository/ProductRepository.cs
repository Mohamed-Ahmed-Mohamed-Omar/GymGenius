using AutoMapper;
using GymGenius.Data;
using GymGenius.Data.Entities;
using GymGenius.Helpers;
using GymGenius.Models;
using GymGenius.Models.Products;
using GymGenius.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymGenius.Services.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGeneral> AddAsync(CreateProduct entity, string UserName)
        {
            var RG = new ResponseGeneral();

            try
            {
                Product data = new Product();

                data.UserName = UserName;

                data.PhotoName = UploadPhoto.SaveFileAsync(entity.PhotoUrl, "Photos");

                await _context.products.AddAsync(data);

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

        public async Task<IEnumerable<GetAllProducts>> GetAllProductsAsync(string UserName)
        {
            var data = await _context.products.Where(m => m.UserName == UserName)
                .Select(s => new GetAllProducts
                {
                    PhotoName = s.PhotoName
                }).ToListAsync();

            return data;
        }

    }
}
