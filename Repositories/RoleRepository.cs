using HelloApi.Data;
using HelloApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private ShopContext _context;

        public RoleRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<Role> Add(Role role)
        {
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
