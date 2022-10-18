using HelloApi.Models;

namespace HelloApi.Repositories
{
    public interface ICategoryRepository
    {
        public Task<Category[]> GetAll();
        public Task<Category> Add(Category category);
    }
}
