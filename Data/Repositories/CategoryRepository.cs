using Microsoft.EntityFrameworkCore;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Data.Repositories
{
	public class CategoryRepository : ARepository<ShopContext>, ICategoryRepository
	{
		public CategoryRepository(ShopContext context) : base(context) { }

		public async Task<IPageData<Category>> Get(CategorySearchParameters searchParameters)
		{
			var query = _context.Categories.OrderBy(i => i.DisplayOrder).AsQueryable();

			if (searchParameters.Name is not null)
			{
				query = query.Where(i => EF.Functions.Like(i.Name, $"{searchParameters.Name}%"));
			}

			if (searchParameters.IsForAdults is not null)
			{
				query = query.Where(i => i.IsForAdults == searchParameters.IsForAdults);
			}

			var totalAmount = await query.CountAsync();
			var data = await query
				.Skip(searchParameters.GetSkip())
				.Take(searchParameters.PageSize)
				.ToArrayAsync();

			var pageData = new PageData<Category>(data, searchParameters.Page, searchParameters.PageSize, totalAmount);
			return pageData;
		}


		public async Task<Category?> GetById(int id)
		{
			var category = await _context.Categories.FirstOrDefaultAsync(i => i.Id == id);
			return category;
		}


		public async Task<Category?> Add(Category category)
		{
			try
			{
				var enityEntry = await _context.Categories.AddAsync(category);
				await _context.SaveChangesAsync();
				return enityEntry.Entity;
			}
			catch (Exception e)
			{
				return null;
			}
		}


		public async Task<Category?> Update(Category category)
		{
			return await Task.Run(() =>
			{
				try
				{
					_context.ChangeTracker.Clear();
					var entityEntry = _context.Categories.Update(category);
					_context.SaveChanges();
					return entityEntry.Entity;
				}
				catch (Exception e)
				{
					return null;
				}
			});
		}


		public async Task<Category?> Delete(int id)
		{
			return await Task.Run(async () =>
			{
				Category? category = await GetById(id);

				if (category is null)
				{
					return null;
				}

				try
				{
					_context.ChangeTracker.Clear();
					var entityEntry = _context.Categories.Remove(category);
					_context.SaveChanges();
					category = entityEntry.Entity;
				}
				catch (Exception e)
				{
					return null;
				}

				return category;
			});
		}
	}
}
