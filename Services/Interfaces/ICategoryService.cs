using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Category;

namespace ShopApi.Services.Interfaces
{
	public interface ICategoryService
	{
		public Task<IPageData<CategoryDTO>> Get(CategorySearchParameters parameters);

		public Task<CategoryDTO?> GetById(int id);

		public Task<CategoryDTO?> Add(CategoryForCreationDTO category);

		public Task<CategoryDTO?> Update(int id, CategoryForUpdateDTO dto);

		public Task<CategoryDTO?> Delete(int id);
	}
}
