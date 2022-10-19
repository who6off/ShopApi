using HelloApi.Data;
using HelloApi.Models;
using HelloApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class ProductRepository : ARepository<ShopContext>, IProductRepository
    {
        public ProductRepository(ShopContext context) : base(context) { }
        public async Task<Product> Add(Product product)
        {
            var newProduct = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return newProduct.Entity;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Update(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<Product[]> GetAll()
        {
            var rasult = await _context.Products.ToArrayAsync<Product>();
            return rasult;
        }
    }
}
