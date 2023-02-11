using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ShopApi.Authentication;
using ShopApi.Data.Models;
using System.Security.Claims;

namespace ShopApi.Authorization
{
	public class ProductAccessRestrictions : AuthorizationHandler<OperationAuthorizationRequirement, Product>
	{
		protected override Task HandleRequirementAsync(
			AuthorizationHandlerContext context,
			OperationAuthorizationRequirement requirement,
			Product product)
		{
			var user = context.User;

			if ((user is null) || (product is null))
			{
				return Task.CompletedTask;
			}

			if (
				requirement.Name != ProductOperationNames.Get &&
				requirement.Name != ProductOperationNames.Add &&
				requirement.Name != ProductOperationNames.Update &&
				requirement.Name != ProductOperationNames.Delete
			)
			{
				return Task.CompletedTask;
			}


			var userRole = user?.GetUserRole();
			var userId = user?.GetUserId();

			if (requirement.Name == ProductOperationNames.Get)
			{
				if (HandleGet(user, userRole, product))
				{
					context.Succeed(requirement);
				};

				return Task.CompletedTask;
			}

			if (!user.Identity.IsAuthenticated)
			{
				return Task.CompletedTask;
			}

			if (requirement.Name == ProductOperationNames.Update)
			{
				if (HandleUpdate(userId, userRole, product))
				{
					context.Succeed(requirement);
				};

				return Task.CompletedTask;
			}

			if (requirement.Name == ProductOperationNames.Delete)
			{
				if (HandleDelete(userId, userRole, product))
				{
					context.Succeed(requirement);
				};

				return Task.CompletedTask;
			}

			return Task.CompletedTask;
		}


		private bool HandleDelete(int? userId, string? userRole, Product product)
		{
			if (userRole == UserRoles.Admin)
			{
				return true;
			}

			if ((userRole == UserRoles.Seller) && (product.SellerId == userId))
			{
				return true;
			}

			return false;
		}


		private bool HandleUpdate(int? userId, string? userRole, Product product)
		{
			if (userRole == UserRoles.Admin)
			{
				return true;
			}

			if ((userRole == UserRoles.Seller) && (product.SellerId == userId))
			{
				return true;
			}

			return false;
		}


		private bool HandleGet(ClaimsPrincipal user, string? userRole, Product product)
		{
			if (user.Identity.IsAuthenticated && (userRole != UserRoles.Buyer))
			{
				return true;
			}

			if ((!user.Identity.IsAuthenticated || !user.IsAdult()) && product.Category.IsForAdults)
			{
				return false;
			}

			return true;
		}
	}
}
