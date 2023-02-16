using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ShopApi.Authorization
{
	public class OrderOperations
	{
		public static readonly OperationAuthorizationRequirement Add =
			new() { Name = OrderOperationNames.Add };

		public static readonly OperationAuthorizationRequirement Get =
			new() { Name = OrderOperationNames.Get };

		public static readonly OperationAuthorizationRequirement Update =
			new() { Name = OrderOperationNames.Update };

		public static readonly OperationAuthorizationRequirement Delete =
			new() { Name = OrderOperationNames.Delete };


		public static readonly OperationAuthorizationRequirement Cancellation =
			new() { Name = OrderOperationNames.Cancellation };

		public static readonly OperationAuthorizationRequirement DeliveryRequest =
			new() { Name = OrderOperationNames.DeliveryRequest };

		public static readonly OperationAuthorizationRequirement DeliveryCompletion =
			new() { Name = OrderOperationNames.DeliveryCompletion };
	}


	static public class OrderOperationNames
	{
		public const string Add = "Add";
		public const string Get = "Get";
		public const string Update = "Update";
		public const string Delete = "Delete";

		public const string Cancellation = "Cancellation";
		public const string DeliveryRequest = "DeliveryRequest";
		public const string DeliveryCompletion = "DeliveryCompletion";
	}
}
