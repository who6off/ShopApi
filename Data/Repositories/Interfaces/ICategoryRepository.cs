using ShopApi.Data.Models;

namespace ShopApi.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<Category?> GetById(int id);
        public IQueryable<Category> GetAll();
        public Task<Category> Add(Category category);
    }
}
