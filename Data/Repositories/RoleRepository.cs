using Microsoft.EntityFrameworkCore;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Data.Repositories
{
	public class RoleRepository : ARepository<ShopContext>, IRoleRepository
	{
		public RoleRepository(ShopContext context) : base(context) { }

		public async Task<Role> Add(Role role)
		{
			var result = await _context.Roles.AddAsync(role);
			await _context.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<IPageData<Role>> Get(RoleSearchParameters parameters)
		{
			var query = _context.Roles.AsQueryable();

			var totalAmount = await query.CountAsync();

			var data = await query
				.Skip(parameters.GetSkip())
				.Take((int)parameters.PageSize)
			.ToArrayAsync();

			var pagedList = new PageData<Role>(data, parameters.Page, parameters.PageSize, (uint)totalAmount);
			return pagedList;
		}
	}
}
