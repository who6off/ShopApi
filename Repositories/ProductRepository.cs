using HelloApi.Data;
using HelloApi.Models;
using HelloApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class ProductRepository : ARepository<ShopContext>, IProductRepository
    {
        public ProductRepository(ShopContext context) : base(context) { }

        public async Task<Product?> GetById(int id)
        {
            var result = await _context.Products.FirstOrDefaultAsync(i => i.Id == id);
            return result;
        }

        public async Task<Product> Add(Product product)
        {
            var newProduct = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return newProduct.Entity;
        }

        public async Task<Product?> Delete(int id)
        {
            return await Task.Run(async () =>
            {
                Product? product = await GetById(id);

                try
                {
                    _context.ChangeTracker.Clear();  //Delete fix
                    product = _context.Products.Remove(product).Entity;
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    return null;
                }

                return product;
            });
        }

        public async Task<Product?> Update(Product product)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _context.ChangeTracker.Clear(); //Update fix
                    var result = _context.Products.Update(product);
                    _context.SaveChanges();
                    return result.Entity;
                }
                catch (Exception e)
                {
                    return null;
                }
            });
        }

        public async Task<Product[]> GetAll()
        {
            var rasult = await _context.Products.ToArrayAsync<Product>();
            return rasult;
        }
    }
}
