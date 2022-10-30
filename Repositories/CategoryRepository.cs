using HelloApi.Data;
using HelloApi.Models;
using HelloApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class CategoryRepository : ARepository<ShopContext>, ICategoryRepository
    {
        public CategoryRepository(ShopContext context) : base(context) { }


        public async Task<Category?> GetById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(i => i.Id == id);
            return category;
        }


        public async Task<Category> Add(Category category)
        {
            var result = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return result.Entity;
        }


        public IQueryable<Category> GetAll()
        {
            var result = _context.Categories.Select(i => i);
            return result;
        }
    }
}
