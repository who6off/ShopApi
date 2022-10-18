using HelloApi.Models;
using HelloApi.Repositories;

namespace HelloApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(
            ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> AddCategory(Category category)
        {
            var result = await _categoryRepository.Add(category);
            return result;
        }

        public async Task<Category[]> GetAllCategories()
        {
            var result = await _categoryRepository.GetAll();
            return result;
        }
    }
}
