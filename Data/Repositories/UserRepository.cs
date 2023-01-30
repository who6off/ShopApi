using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers;

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


		public async Task<PagedList<User>> Get(UserSearchParameters searchParameters)
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

			var pagedList = new PagedList<User>(data, searchParameters.Page, searchParameters.PageSize, (uint)totalAmount);
			return pagedList;
		}
	}
}
