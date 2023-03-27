using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Data.Repositories.Interfaces
{
	public interface ICategoryRepository
	{
		public Task<Category?> GetById(int id);
		public Task<IPageData<Category>> Get(CategorySearchParameters parameters);
		public Task<Category?> Add(Category category);
		public Task<Category?> Update(Category category);
		public Task<Category?> Delete(int id);
	}
}
