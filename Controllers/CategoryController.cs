using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Authentication;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Models.DTOs.Category;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;

		public CategoryController(
			ICategoryService categoryService,
			IMapper mapper
			)
		{
			_categoryService = categoryService;
			_mapper = mapper;
		}


		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Get([FromQuery] CategorySearchParameters parameters)
		{
			var categories = await _categoryService.Get(parameters);
			var categoriesMap = categories.Map<CategoryDTO>(_mapper);
			return Ok(categoriesMap);
		}


		[HttpPost]
		[Authorize(Roles = $"{UserRoles.Admin}")]
		public async Task<IActionResult> Add([FromBody] CategoryForCreationDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var category = _mapper.Map<Category>(dto);
			var newCategory = await _categoryService.Add(category);

			if (newCategory is null)
			{
				throw new Exception("Creation error");
			}

			var categoryDTO = _mapper.Map<CategoryDTO>(newCategory);

			return Ok(categoryDTO);
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
				throw new Exception("Update error");
			}

			var categoryDTO = _mapper.Map<CategoryDTO>(updatedCategory);

			return Ok(categoryDTO);
		}


		[HttpDelete]
		[Authorize(Roles = $"{UserRoles.Admin}")]
		[Route("{id:required}")]
		public async Task<IActionResult> Delete(int id)
		{
			var deletedCategory = await _categoryService.Delete(id);

			if (deletedCategory is null)
			{
				throw new Exception("Delete error");
			}

			var categoryDTO = _mapper.Map<CategoryDTO>(deletedCategory);

			return Ok(categoryDTO);
		}
	}
}
