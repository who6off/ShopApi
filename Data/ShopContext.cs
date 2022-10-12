using HelloApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Data
{
    public class ShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }


    }
}
