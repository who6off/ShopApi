using HelloApi.Data;
using HelloApi.Extensions;
using HelloApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class RoleRepository : ARepository<ShopContext>, IRoleRepository
    {
        public RoleRepository(ShopContext context) : base(context) { }

        public async Task<Role> Add(Role role)
        {
            role.Name.FirstCharToUpper();
            var result = await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Role[]> GetAll()
        {
            var result = await _context.Roles.ToArrayAsync<Role>();
            return result;
        }
    }
}
