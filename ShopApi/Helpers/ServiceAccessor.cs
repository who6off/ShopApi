namespace ShopApi.Helpers
{
	public static class ServiceAccessor
	{
		private static IServiceProvider? _serviceProvider = null;


		public static IServiceProvider? Services
		{
			get => _serviceProvider;
			set
			{
				if (_serviceProvider is null)
				{
					_serviceProvider = value;
				}
			}
		}


		public static T? Get<T>()
		{
			return _serviceProvider.GetService<T>();
		}
	}
}
