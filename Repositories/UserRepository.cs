using HelloApi.Data;
using HelloApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopContext _context;

        public UserRepository(ShopContext context)
        {
            _context = context;
        }
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

        public async Task<User[]> GetAll()
        {
            var result = await _context
                .Users
                .Include(u => u.Role)
                .ToArrayAsync<User>();
            return result;
        }
    }
}
