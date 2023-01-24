using ShopApi.Data.Models;

namespace ShopApi.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product?> GetById(int id);
        public Task<string?> GetImageById(int id);
        public Task<Product[]> GetByCategory(int categoryId);
        public IQueryable<Product> GetAll();


        public Task<Product> Add(Product product);
        public Task<Product?> Update(Product product);
        public Task<Product?> Delete(int id);
    }
}
