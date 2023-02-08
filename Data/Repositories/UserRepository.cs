using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Repositories
{
	public class UserRepository : ARepository<ShopContext>, IUserRepository
	{
		public UserRepository(ShopContext context) : base(context) { }
		public async Task<User> Add(User user)
		{
			var result = await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			await _context.Entry(result.Entity).Reference(u => u.Role).LoadAsync();
			return result.Entity;
		}

		public async Task<User?> FindByEmail(string email)
		{
			var result = await _context
				.Users
				.Include(i => i.Role)
				.FirstOrDefaultAsync(i => i.Email == email);
			return result;
		}

		public async Task<User?> GetById(int id)
		{
			var result = await _context
			   .Users
			   .Include(i => i.Role)
			   .FirstOrDefaultAsync(i => i.Id == id);
			return result;
		}


		public async Task<IPageData<User>> Get(UserSearchParameters searchParameters)
		{
			var query = _context
				.Users
				.Include(i => i.Role)
				.AsQueryable();

			var totalAmount = await query.CountAsync();

			var data = await query
				.Skip(searchParameters.GetSkip())
				.Take((int)searchParameters.PageSize)
				.ToArrayAsync();

			var pageData = new PageData<User>(data, searchParameters.Page, searchParameters.PageSize, totalAmount);
			return pageData;
		}


		public async Task<User?> Update(User user)
		{
			return await Task.Run(async () =>
			{
				try
				{
					_context.ChangeTracker.Clear();
					var enityEntry = _context.Users.Update(user);
					_context.SaveChanges();
					return await GetById(user.Id);
				}
				catch (Exception e)
				{
					return null;
				}
			});
		}


		public async Task<User?> Delete(int id)
		{
			return await Task.Run(async () =>
			{
				try
				{
					var user = await GetById(id);

					_context.ChangeTracker.Clear();
					var enityEntry = _context.Users.Remove(user);
					_context.SaveChanges();
					return user;
				}
				catch (Exception e)
				{
					return null;
				}
			});
		}
	}
}
