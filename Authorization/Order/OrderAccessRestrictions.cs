using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ShopApi.Authentication;
using ShopApi.Data.Models;

namespace ShopApi.Authorization
{
	public class OrderAccessRestrictions : AuthorizationHandler<OperationAuthorizationRequirement, Order>
	{
		protected override Task HandleRequirementAsync(
			AuthorizationHandlerContext context,
			OperationAuthorizationRequirement requirement,
			Order order
		)
		{
			var user = context.User;
			var userId = user?.GetUserId();
			var userRole = user?.GetUserRole();

			if ((user is null) || !user.Identity.IsAuthenticated || string.IsNullOrEmpty(userRole) || (userId is null))
			{
				return Task.CompletedTask;
			}

			if (order is null)
			{
				return Task.CompletedTask;
			}

			if (
				requirement.Name != OrderOperationNames.Get &&
				requirement.Name != OrderOperationNames.Add &&
				requirement.Name != OrderOperationNames.Update &&
				requirement.Name != OrderOperationNames.Delete &&
				requirement.Name != OrderOperationNames.Cancellation &&
				requirement.Name != OrderOperationNames.DeliveryRequest &&
				requirement.Name != OrderOperationNames.DeliveryCompletion
			)
			{
				return Task.CompletedTask;
			}

			var isAuthorized = requirement.Name switch
			{
				OrderOperationNames.Get => HandleGet(userId.Value, userRole, order),
				OrderOperationNames.Update => HandleUpdate(userId.Value, userRole, order),
				OrderOperationNames.Delete => HandleDelete(userId.Value, userRole, order),
				OrderOperationNames.Cancellation => HandleCancellation(userId.Value, userRole, order),
				OrderOperationNames.DeliveryRequest => HandleDeliveryRequest(userId.Value, order),
				OrderOperationNames.DeliveryCompletion => HandleDeliveryCompletion(userRole),
				_ => true
			};

			if (isAuthorized)
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}


		private bool HandleGet(int userId, string userRole, Order order)
		{
			if (userRole == UserRoles.Admin)
			{
				return true;
			}

			if (userId == order.BuyerId)
			{
				return true;
			}

			return false;
		}


		private bool HandleUpdate(int userId, string userRole, Order order)
		{
			if (userId == order.BuyerId)
			{
				return true;
			}

			return false;
		}


		private bool HandleDelete(int userId, string userRole, Order order)
		{
			if (userRole == UserRoles.Admin)
			{
				return true;
			}

			if (userId == order.BuyerId)
			{
				return true;
			}

			return false;
		}


		private bool HandleCancellation(int userId, string userRole, Order order)
		{
			if (userRole == UserRoles.Admin)
			{
				return true;
			}

			if (userId == order.BuyerId)
			{
				return true;
			}

			return false;
		}


		private bool HandleDeliveryRequest(int userId, Order order)
		{
			if (userId == order.BuyerId)
			{
				return true;
			}

			return false;
		}


		private bool HandleDeliveryCompletion(string userRole)
		{
			if (userRole == UserRoles.Admin)
			{
				return true;
			}

			return false;
		}
	}
}

