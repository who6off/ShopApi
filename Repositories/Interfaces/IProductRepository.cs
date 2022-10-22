using HelloApi.Models;

namespace HelloApi.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product?> GetById(int id);
        public Task<Product[]> GetAll();


        public Task<Product> Add(Product product);
        public Task<Product?> Update(Product product);
        public Task<Product?> Delete(int id);
    }
}
