using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShopApi.Authentication;
using ShopApi.Authentication.Interfaces;
using ShopApi.Configuration;
using ShopApi.Data.Models;

namespace ShopApi.Data
{
	public class ShopContext : DbContext
	{
		private readonly ShopDbSettings _settings;
		private readonly IPasswordHasher _passwordHasher;

		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }

		public ShopContext(
			DbContextOptions<ShopContext> options,
			IOptions<ShopDbSettings> settings,
			IPasswordHasher passwordHasher) : base(options)
		{
			_settings = settings.Value;
			_passwordHasher = passwordHasher;

			if (_settings.InitData)
			{
				Database.EnsureDeleted();
				Database.EnsureCreated();
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique();


			modelBuilder.Entity<User>()
				.HasOne(u => u.Role)
				.WithMany(r => r.Users)
				.HasForeignKey(u => u.RoleId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();


			modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();


			modelBuilder.Entity<Product>()
				.HasOne(p => p.Category)
				.WithMany(c => c.Products)
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Seller)
				.WithMany(u => u.Products)
				.HasForeignKey(p => p.SellerId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<Order>()
				.HasOne(o => o.Buyer)
				.WithMany(u => u.Orders)
				.HasForeignKey(o => o.BuyerId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Order>()
			  .HasOne(o => o.Buyer)
			  .WithMany(u => u.Orders)
			  .HasForeignKey(o => o.BuyerId)
			  .OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Order>()
				.HasMany(o => o.OrderItems)
				.WithOne(oi => oi.Order)
				.HasForeignKey(oi => oi.OrderId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Order)
				.WithMany(o => o.OrderItems)
				.HasForeignKey(oi => oi.OrderId);

			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Product)
				.WithMany(p => p.OrderItems)
				.HasForeignKey(oi => oi.ProductId);

			InitData(modelBuilder);
		}

		private void InitData(ModelBuilder modelBuilder)
		{
			if (!_settings.InitData) return;

			var roles = new Role[]
			{
				new Role(){Id = 1, Name = UserRoles.Admin},
				new Role(){Id = 2, Name = UserRoles.Seller},
				new Role(){Id = 3, Name = UserRoles.Buyer}
			};

			var mainAdmin = new User()
			{
				Id = 1,
				Email = _settings.MainAdmin.Email,
				PasswordHash = _passwordHasher.Hash(_settings.MainAdmin.Password),
				RoleId = 1,
				FirstName = _settings.MainAdmin.FirstName,
				SecondName = _settings.MainAdmin.SecondName,
				BirthDate = _settings.MainAdmin.BirthDate
			};

			modelBuilder.Entity<Role>().HasData(roles);

			modelBuilder.Entity<User>().HasData(mainAdmin);
		}
	}
}
