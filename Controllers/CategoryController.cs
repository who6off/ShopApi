using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Authentication;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Exceptions;
using ShopApi.Models.DTOs.Category;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(
			ICategoryService categoryService
			)
		{
			_categoryService = categoryService;
		}


		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Get([FromQuery] CategorySearchParameters parameters)
		{
			var categories = await _categoryService.Get(parameters);

			return Ok(categories);
		}


		[HttpGet]
		[AllowAnonymous]
		[Route("{id:required}")]
		public async Task<IActionResult> GetById(int id)
		{
			var category = await _categoryService.GetById(id);

			if (category is null)
			{
				throw new NotFoundException("This category is not found!");
			}

			return Ok(category);
		}


		[HttpPost]
		[Authorize(Roles = $"{UserRoles.Admin}")]
		public async Task<IActionResult> Add([FromBody] CategoryForCreationDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var newCategory = await _categoryService.Add(dto);

			if (newCategory is null)
			{
				throw new AppException("Creation error!");
			}

			return Ok(newCategory);
		}


		[HttpPut]
		[Authorize(Roles = $"{UserRoles.Admin}")]
		[Route("{id:required}")]
		public async Task<IActionResult> Update(int id, CategoryForUpdateDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var updatedCategory = await _categoryService.Update(id, dto);

			if (updatedCategory is null)
			{
				throw new AppException("Update error!");
			}

			return Ok(updatedCategory);
		}


		[HttpDelete]
		[Authorize(Roles = $"{UserRoles.Admin}")]
		[Route("{id:required}")]
		public async Task<IActionResult> Delete(int id)
		{
			var deletedCategory = await _categoryService.Delete(id);

			if (deletedCategory is null)
			{
				throw new AppException("Delete error!");
			}

			return Ok(deletedCategory);
		}
	}
}
