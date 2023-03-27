using Microsoft.EntityFrameworkCore;
using ShopApi.Data.Models;
using ShopApi.Data.Repositories.Interfaces;

namespace ShopApi.Data.Repositories
{
	public class RoleRepository : ARepository<ShopContext>, IRoleRepository
	{
		public RoleRepository(ShopContext context) : base(context) { }

		public async Task<Role[]> GetAll()
		{
			var roles = await _context.Roles.ToArrayAsync();

			return roles;
		}


		public async Task<Role?> GetById(int id)
		{
			var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
			return role;
		}


		public async Task<Role?> GetByName(string name)
		{
			var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
			return role;
		}
	}
}
