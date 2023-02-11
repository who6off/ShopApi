using Microsoft.EntityFrameworkCore;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Data.Repositories
{
	public class ProductRepository : ARepository<ShopContext>, IProductRepository
	{
		public ProductRepository(ShopContext context) : base(context) { }


		public async Task<IPageData<Product>> Get(ProductSearchParameters searchParameters)
		{
			var query = _context.Products
				.Include(i => i.Category)
				.AsQueryable();

			var totalAmount = await query.CountAsync();
			var data = await query
				.Skip(searchParameters.GetSkip())
				.Take(searchParameters.PageSize)
				.ToArrayAsync();

			var pageData = new PageData<Product>(data, searchParameters.Page, searchParameters.PageSize, totalAmount);
			return pageData;
		}


		public async Task<Product?> GetById(int id)
		{
			var result = await _context
				.Products
				.Include(i => i.Category)
				.FirstOrDefaultAsync(i => i.Id == id);
			return result;
		}

		public async Task<Product?> Add(Product product)
		{
			try
			{
				var entityEntry = await _context.Products.AddAsync(product);
				await _context.SaveChangesAsync();
				await _context.Entry(entityEntry.Entity).Reference(i => i.Category).LoadAsync();
				return entityEntry.Entity;
			}
			catch (Exception e)
			{
				return null;
			}
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
					//TODO: Add Log!
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
					var entityEntry = _context.Products.Update(product);
					_context.SaveChanges();
					_context.Entry(entityEntry.Entity).Reference(i => i.Category).Load();
					return entityEntry.Entity;
				}
				catch (Exception e)
				{
					//TODO: Add Log
					return null;
				}
			});
		}
	}
}
