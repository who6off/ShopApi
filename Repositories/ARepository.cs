using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
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
