using Microsoft.EntityFrameworkCore;
using ShopApi.Configuration;

namespace ShopApi.Data
{
	static public class DatabaseInitializer
	{
		static public void Initialize(IServiceProvider serviceProvider)
		{
			var configuration = serviceProvider.GetService<IConfiguration>();
			var dbSettings = configuration.GetSection("ShopDbSettings").Get<ShopDbSettings>();

			if (dbSettings.InitData)
			{
				using (var scope = serviceProvider.CreateScope())
				{
					var shopContext = scope.ServiceProvider.GetService<ShopContext>();
					shopContext.Database.Migrate();
					shopContext.Database.EnsureCreated();
				}
			}

		}
	}
}
