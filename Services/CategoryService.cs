using AutoMapper;
using ShopApi.Authorization;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers.Exceptions;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Category;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CategoryService(
			ICategoryRepository repository,
			IMapper mapper,
			IHttpContextAccessor httpContextAccessor)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}


		public async Task<CategoryDTO?> Add(CategoryForCreationDTO dto)
		{
			var category = _mapper.Map<Category>(dto);
			var newCategory = await _repository.Add(category);
			var newCategoryDTO = _mapper.Map<CategoryDTO>(newCategory);
			return newCategoryDTO;
		}


		public async Task<CategoryDTO?> Update(int id, CategoryForUpdateDTO dto)
		{
			var category = await _repository.GetById(id);

			if (category is null)
			{
				throw new NotFoundException("This category is not found!");
			}

			_mapper.Map(dto, category);
			var updatedCategory = await _repository.Update(category);
			var updatedCategoryDTO = _mapper.Map<CategoryDTO>(updatedCategory);
			return updatedCategoryDTO;
		}


		public async Task<CategoryDTO?> Delete(int id)
		{
			var deletedCategory = await _repository.Delete(id);
			var deletedCategoryDTO = _mapper.Map<CategoryDTO>(deletedCategory);
			return deletedCategoryDTO;
		}


		public async Task<IPageData<CategoryDTO>> Get(CategorySearchParameters parameters)
		{
			var user = _httpContextAccessor.HttpContext.User;

			if (
				(parameters.IsForAdults is not null) &&
				(((user is null) && parameters.IsForAdults.Value) ||
				((user is not null) && (user.IsAdult() == false) && parameters.IsForAdults.Value))
			)
			{
				throw new AccessDeniedException("Access denied");
			}

			if (
				(parameters.IsForAdults is null) &&
				((user is null) || (user.IsAdult() == false))
			)
			{
				parameters.IsForAdults = false;
			}

			var data = await _repository.Get(parameters);
			var dataMap = data.Map<CategoryDTO>(_mapper);
			dataMap.Data = dataMap.Data.OrderBy(i => i.DisplayOrder);
			return dataMap;
		}


		public async Task<CategoryDTO?> GetById(int id)
		{
			var category = await _repository.GetById(id);

			if (category is null)
			{
				return null;
			}

			var user = _httpContextAccessor.HttpContext.User;

			if (category.IsForAdults && ((user is null) || (user?.IsAdult() == false)))
			{
				throw new AccessDeniedException("Access denied");
			}

			var categoryDTO = _mapper.Map<CategoryDTO>(category);
			return categoryDTO;
		}
	}
}
