using GymGenius.Models.Identity;
using GymGenius.Models.Products;
using GymGenius.Models.Users;
using GymGenius.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymGenius.Controllers
{
    [Authorize(Roles = clsRoles.roleUser)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductRepository _productRepository;

        public ProductController(UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _userManager = userManager;
            _productRepository = productRepository;
        }

        [HttpPost("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto([FromForm]CreateProduct product)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(username);

            var data = await _productRepository.AddAsync(product, user.UserName);

            return Ok(data);
        }

        [HttpGet("ShowPhotos")]
        public async Task<ActionResult<IEnumerable<GetAllProducts>>> GetAllPhotos()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(username);

            var data = await _productRepository.GetAllProductsAsync(user.UserName);

            return Ok(data);
        }
    }
}
