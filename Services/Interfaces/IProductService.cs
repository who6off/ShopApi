using ShopApi.Data.Models;
using ShopApi.Models.DTOs.Product;

namespace ShopApi.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDTO?> Add(ProductForCreationDTO dto);

        public Task<ProductDTO?> Update(int id, ProductForUpdateDTO dto);

        public Task<bool> Delete(int id);

        public Task<int?> GetSellerIdByProductId(int id);

        public Task<Product[]> GetAll();
        public Task<Product[]> GetByCategory(int categoryId);
        public Task<Product?> GetById(int id);

        public Task<Category?> GetCategoryById(int id);
        public Task<Category> AddCategory(Category category);
        public Task<Category[]> GetCategories(bool isAdult);
    }
}
