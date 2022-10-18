using HelloApi.Authentication;
using HelloApi.Authorization;
using HelloApi.Models;
using HelloApi.Models.Requests;
using HelloApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(
            IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return Ok(new { Message = "Products" });
        }

        [HttpGet]
        [Route("adults")]
        [Authorize(Policy = AgeRestrictionPolicy.Name)]
        public IActionResult Adult()
        {
            return Ok(new { Message = "Adult Products" });
        }

        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _productService.GetAllCategories();
            return (result is null) ? BadRequest() : Ok(result);
        }


        [HttpPost]
        [Route("category")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateCategory(CategoryCreationRequest req)
        {
            var result = await _productService.AddCategory(new Category()
            {
                Name = req.Name,
                IsForAdults = req.IsForAdults
            });
            return (result is null) ? BadRequest() : Ok(result);
        }
    }
}

