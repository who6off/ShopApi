using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Category;

namespace ShopApi.Services.Interfaces
{
	public interface ICategoryService
	{
		public Task<IPageData<Category>> Get(CategorySearchParameters parameters);

		public Task<Category?> GetById(int id);

		public Task<Category?> Add(Category category);

		public Task<Category?> Update(int id, CategoryForUpdateDTO dto);

		public Task<Category?> Delete(int id);
	}
}
