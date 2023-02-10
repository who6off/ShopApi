using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Authentication;
using ShopApi.Authorization;
using ShopApi.Helpers.Exceptions;
using ShopApi.Models.DTOs.Product;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
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
		public async Task<IActionResult> GetAllProducts()
		{

			var result = await _productService.GetAll();
			return Ok(result);
		}


		//[HttpGet]
		//[Route("category/{id}")]
		//public async Task<IActionResult> GetProductsByCategory(int id)
		//{
		//	var category = await _productService.GetCategoryById(id);
		//	if (category is null)
		//		return NotFound();

		//	if (
		//		(category.IsForAdults && !HttpContext.User.Identity.IsAuthenticated) ||
		//		!HttpContext.User.IsAdult())
		//		return StatusCode(StatusCodes.Status403Forbidden);

		//	var result = await _productService.GetByCategory(id);
		//	return Ok(result);
		//}


		[HttpPost]
		[Consumes("multipart/form-data")]
		[Authorize(Roles = UserRoles.Seller)]
		public async Task<IActionResult> CreateProduct([FromForm] ProductForCreationDTO dto)
		{
			var result = await _productService.Add(dto);

			return (result is null) ? BadRequest() : Ok(result);
		}


		[HttpPut]
		[Consumes("multipart/form-data")]
		[Authorize(Roles = UserRoles.Seller)]
		[Route("{id:required}")]
		public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromForm] ProductForUpdateDTO dto)
		{
			var updetedProduct = await _productService.Update(id, dto);

			if (updetedProduct is null)
			{
				throw new AppException("Update error!");
			}

			return Ok(updetedProduct);
		}


		[HttpDelete]
		[Route("{id}")]
		[Authorize(Roles = $"{UserRoles.Seller}, {UserRoles.Admin}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var role = HttpContext.User.GetUserRole();
			//if (role == UserRoles.Seller && !(await IsPermitedSeller(id)))
			//	return StatusCode(StatusCodes.Status403Forbidden);

			var isDeleted = await _productService.Delete(id);

			return isDeleted ? Ok() : BadRequest();
		}


		//[HttpGet]
		//[Route("category")]
		//public async Task<IActionResult> GetNonAdultCategories()
		//{
		//	var result = await _productService.GetCategories(false);
		//	return (result is null) ? BadRequest() : Ok(result);
		//}


		//[HttpGet]
		//[Route("category/adult")]
		//[Authorize(Policy = AgeRestrictionPolicy.Name)]
		//public async Task<IActionResult> GetAdultCategories()
		//{
		//	var result = await _productService.GetCategories(true);
		//	return (result is null) ? BadRequest() : Ok(result);
		//}


		//[HttpPost]
		//[Route("category")]
		//[Authorize(Roles = UserRoles.Admin)]
		//public async Task<IActionResult> CreateCategory(CategoryCreationRequest req)
		//{
		//	var result = await _productService.AddCategory(new Category()
		//	{
		//		Name = req.Name,
		//		IsForAdults = req.IsForAdults
		//	});
		//	return (result is null) ? BadRequest() : Ok(result);
		//}


		//[ApiExplorerSettings(IgnoreApi = true)]
		//private async Task<bool> IsPermitedSeller(int productId)
		//{
		//	var userId = HttpContext.User.GetUserId();
		//	var sellerId = (await _productService.GetSellerIdByProductId(productId)) ?? 0;
		//	return userId == sellerId;
		//}
	}
}

