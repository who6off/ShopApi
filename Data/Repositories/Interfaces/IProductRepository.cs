using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Data.Repositories.Interfaces
{
	public interface IProductRepository
	{
		public Task<IPageData<Product>> Get(ProductSearchParameters seachParameters);
		public Task<Product?> GetById(int id);
		public Task<Product> Add(Product product);
		public Task<Product?> Update(Product product);
		public Task<Product?> Delete(int id);
	}
}
