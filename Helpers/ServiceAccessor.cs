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
				if (_serviceProvider == null)
				{
					_serviceProvider = value;
				}
			}
		}


		public static T? Get<T>()
		{
			if (_serviceProvider == null)
			{
				return null;
			}

			return _serviceProvider.GetService<T>();
		}
	}
}
