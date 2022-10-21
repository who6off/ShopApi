using HelloApi.Models;

namespace HelloApi.Services.Interfaces
{
    public interface IProductService
    {
        public Task<Product> Add(Product product, IFormFile? image = null);
        public Task<Product> Update(Product product, IFormFile? image = null);
        public Task<Product> Delete(int id);

        public Task<int?> GetSellerIdByProductId(int id);

        public Task<Product[]> GetAll();

        public Task<Category> AddCategory(Category category);

        public Task<Category[]> GetAllCategories();


    }
}
