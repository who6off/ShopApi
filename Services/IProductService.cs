using HelloApi.Models;

namespace HelloApi.Services
{
    public interface IProductService
    {
        public Task<Category> AddCategory(Category category);

        public Task<Category[]> GetAllCategories();
    }
}
