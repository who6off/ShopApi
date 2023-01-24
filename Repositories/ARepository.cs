using Microsoft.EntityFrameworkCore;

namespace ShopApi.Repositories
{
    public abstract class ARepository<T> where T : DbContext
    {
        protected readonly T _context;

        public ARepository(T context)
        {
            _context = context;
        }
    }
}
