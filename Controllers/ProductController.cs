using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Authentication;
using ShopApi.Data.Models.SearchParameters;
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
		[AllowAnonymous]
		public async Task<IActionResult> GetProducts([FromQuery] ProductSearchParameters searchParameters)
		{
			var products = await _productService.Get(searchParameters);
			return Ok(products);
		}


		[HttpGet]
		[AllowAnonymous]
		[Route("{id:required}")]
		public async Task<IActionResult> GetProductById([FromRoute] int id)
		{
			var product = await _productService.GetById(id);
			return Ok(product);
		}



		[HttpPost]
		[Consumes("multipart/form-data")]
		[Authorize(Roles = $"{UserRoles.Seller}, {UserRoles.Admin}")]
		public async Task<IActionResult> AddProduct([FromForm] ProductForCreationDTO dto)
		{
			var product = await _productService.Add(dto);

			if (product is null)
			{
				throw new AppException("Creation error!");
			}

			return Ok(product);
		}


		[HttpPut]
		[Consumes("multipart/form-data")]
		[Authorize(Roles = $"{UserRoles.Seller}, {UserRoles.Admin}")]
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
		[Authorize(Roles = $"{UserRoles.Seller}, {UserRoles.Admin}")]
		[Route("{id:required}")]
		public async Task<IActionResult> DeleteProduct([FromRoute] int id)
		{
			var deletedProduct = await _productService.Delete(id);

			if (deletedProduct is null)
			{
				throw new AppException("Delete error!");
			}

			return Ok(deletedProduct);
		}
	}
}

