using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Product;

namespace ShopApi.Services.Interfaces
{
	public interface IProductService
	{
		public Task<IPageData<ProductDTO>> Get(ProductSearchParameters searchParameters);

		public Task<ProductDTO?> GetById(int id);

		public Task<ProductDTO?> Add(ProductForCreationDTO dto);

		public Task<ProductDTO?> Update(int id, ProductForUpdateDTO dto);

		public Task<ProductDTO?> Delete(int id);
	}
}
