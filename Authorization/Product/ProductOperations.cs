using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ShopApi.Authorization
{
	public class ProductOperations
	{
		public static readonly OperationAuthorizationRequirement Add =
			new() { Name = ProductOperationNames.Add };

		public static readonly OperationAuthorizationRequirement Get =
			new() { Name = ProductOperationNames.Get };

		public static readonly OperationAuthorizationRequirement Update =
			new() { Name = ProductOperationNames.Update };

		public static readonly OperationAuthorizationRequirement Delete =
			new() { Name = ProductOperationNames.Delete };
	}


	static public class ProductOperationNames
	{
		public const string Add = "Add";
		public const string Get = "Get";
		public const string Update = "Update";
		public const string Delete = "Delete";
	}
}
