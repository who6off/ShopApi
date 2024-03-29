﻿using Microsoft.EntityFrameworkCore;
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
				.Include(i => i.Seller)
				.AsQueryable();

			if (searchParameters.Name is not null)
			{
				query = query.Where(i => EF.Functions.Like(i.Name, $"{searchParameters.Name}%"));
			}

			if (searchParameters.MinPrice is not null)
			{
				query = query.Where(i => i.Price >= searchParameters.MinPrice);
			}

			if (searchParameters.MaxPrice is not null)
			{
				query = query.Where(i => i.Price <= searchParameters.MaxPrice);
			}

			if (searchParameters.IsForAdults is not null)
			{
				query = query
					.Where(i => i.Category != null)
					.Where(i => i.Category!.IsForAdults == searchParameters.IsForAdults);
			}

			if (searchParameters.Categories is not null)
			{
				query = query
					.Where(i => i.CategoryId != null)
					.Where(i => searchParameters.Categories.Contains(i.CategoryId.Value));
			}

			if (searchParameters.Sellers is not null)
			{
				query = query.Where(i => searchParameters.Sellers.Contains(i.SellerId));
			}

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
				.Include(i => i.Seller)
				.FirstOrDefaultAsync(i => i.Id == id);
			return result;
		}


		public async Task<Product[]> GetRangeById(IEnumerable<int> ids)
		{
			var result = await _context
				.Products
				.Where(i => ids.Contains(i.Id))
				.Include(i => i.Category)
				.Include(i => i.Seller)
				.ToArrayAsync();

			return result;
		}


		public async Task<Product?> Add(Product product)
		{
			try
			{
				var entityEntry = await _context.Products.AddAsync(product);
				await _context.SaveChangesAsync();

				await _context.Entry(entityEntry.Entity).Reference(i => i.Category).LoadAsync();
				await _context.Entry(entityEntry.Entity).Reference(i => i.Seller).LoadAsync();

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

				if (product is null)
				{
					return null;
				}

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
					_context.Entry(entityEntry.Entity).Reference(i => i.Seller).Load();

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
