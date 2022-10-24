﻿using HelloApi.Authentication;
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
        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = UserRoles.Seller)]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreationRequest request)
        {
            var sellerId = HttpContext.User.GetUserId();
            if (sellerId is null) return StatusCode(StatusCodes.Status403Forbidden);

            var result = await _productService.Add(new Product()
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
                SellerId = sellerId.Value,
            }, request.Image);

            return (result is null) ? BadRequest() : Ok(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = UserRoles.Seller)]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateRequest request)
        {
            if (!(await IsPermitedSeller(request.Id)))
                return StatusCode(StatusCodes.Status403Forbidden);

            var updetedProduct = await _productService.Update(new Product()
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
                SellerId = HttpContext.User.GetUserId().Value
            }, request.NewImage);

            return (updetedProduct is null) ? BadRequest() : Ok(updetedProduct);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = $"{UserRoles.Seller}, {UserRoles.Admin}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var role = HttpContext.User.GetUserRole();
            if (role == UserRoles.Seller && !(await IsPermitedSeller(id)))
                return StatusCode(StatusCodes.Status403Forbidden);

            var isDeleted = await _productService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
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
            var userId = HttpContext.User.GetUserId();
            var sellerId = (await _productService.GetSellerIdByProductId(productId)) ?? 0;
            return userId == sellerId;
        }
    }
}

