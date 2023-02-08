using AutoMapper;
using ShopApi.Helpers.Exceptions;
using ShopApi.Authorization;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
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


		public async Task<Category?> Add(Category category)
		{
			var newCategory = await _repository.Add(category);
			return newCategory;
		}


		public async Task<Category?> Update(int id, CategoryForUpdateDTO dto)
		{
			var category = await _repository.GetById(id);

			if (category is null)
			{
				throw new NotFoundException("This category is not found!");
			}

			_mapper.Map(dto, category);
			var updatedCategory = await _repository.Update(category);

			return updatedCategory;
		}


		public async Task<Category?> Delete(int id)
		{
			var deletedCategory = await _repository.Delete(id);
			return deletedCategory;
		}


		public Task<IPageData<Category>> Get(CategorySearchParameters parameters)
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

			return _repository.Get(parameters);
		}


		public async Task<Category?> GetById(int id)
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

			return category;
		}
	}
}
