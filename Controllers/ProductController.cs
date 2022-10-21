using HelloApi.Authentication;
using HelloApi.Authorization;
using HelloApi.Models;
using HelloApi.Models.Requests;
using HelloApi.Services.Interfaces;
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
        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = UserRoles.Seller)]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreationRequest req)
        {
            var sellerId = HttpContext.GetUserId();
            if (sellerId is null) return BadRequest();

            var result = await _productService.Add(new Product()
            {
                Name = req.Name,
                Price = req.Price,
                CategoryId = req.CategoryId,
                SellerId = sellerId.Value,
            }, req.Image);

            return (result is null) ? BadRequest() : Ok(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = UserRoles.Seller)]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateRequest req)
        {
            if (!(await IsPermitedSeller(req.Id)))
                return BadRequest();

            var updetedProduct = await _productService.Update(new Product()
            {
                Id = req.Id,
                Name = req.Name,
                Price = req.Price,
                CategoryId = req.CategoryId,
                SellerId = HttpContext.GetUserId().Value
            }, req.NewImage);

            return Ok(updetedProduct);
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

        private async Task<bool> IsPermitedSeller(int productId)
        {
            var userId = HttpContext.GetUserId();
            var sellerId = (await _productService.GetSellerIdByProductId(productId)) ?? 0;
            return userId == sellerId;
        }
    }
}

