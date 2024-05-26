using GymGenius.Models;
using GymGenius.Models.Products;

namespace GymGenius.Services.Interface
{
    public interface IProductRepository
    {
        Task<ResponseGeneral> AddAsync(CreateProduct entity, string UserName);

        Task<IEnumerable<GetAllProducts>> GetAllProductsAsync(string UserName);
    }
}
