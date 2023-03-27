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
		public async Task<User?> Add(User user)
		{
			try
			{
				var result = await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();
				await _context.Entry(result.Entity).Reference(u => u.Role).LoadAsync();
				return result.Entity;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public async Task<User?> GetByEmail(string email)
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

			if (searchParameters.FirstName is not null)
			{
				query = query.Where(i => EF.Functions.Like(i.FirstName, $"{searchParameters.FirstName}%"));
			}

			if (searchParameters.SecondName is not null)
			{
				query = query.Where(i => EF.Functions.Like(i.FirstName, $"{searchParameters.FirstName}%"));
			}

			if (searchParameters.Email is not null)
			{
				query = query.Where(i => i.Email == searchParameters.Email);
			}

			if (searchParameters.RoleId is not null)
			{
				query = query.Where(i => i.RoleId == searchParameters.RoleId);
			}

			if (searchParameters.BirthDate is not null)
			{
				query = query.Where(i => i.BirthDate == searchParameters.BirthDate);
			}

			var totalAmount = await query.CountAsync();
			var data = await query
				.Skip(searchParameters.GetSkip())
				.Take(searchParameters.PageSize)
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


		public async Task<string?> GetUserRoleNameById(int id)
		{
			var result = await _context
				.Users
				.Include(i => i.Role)
				.Where(i => i.Id == id)
				.Select(i => i.Role.Name)
				.FirstOrDefaultAsync();

			return result;
		}
	}
}
