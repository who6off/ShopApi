using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ShopApi.Authentication;
using ShopApi.Data.Models;

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
			var isAdult = user?.IsAdult() ?? false;
			var isAuthenticated = user.Identity.IsAuthenticated;

			var isAuthorized = requirement.Name switch
			{
				ProductOperationNames.Get => HandleGet(isAuthenticated, isAdult, userRole, product),
				ProductOperationNames.Add => HandleAdd(isAuthenticated, userRole),
				ProductOperationNames.Update => HandleUpdate(isAuthenticated, userId, userRole, product),
				ProductOperationNames.Delete => HandleDelete(isAuthenticated, userId, userRole, product),
				_ => true
			};

			if (isAuthorized)
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}


		private bool HandleDelete(bool isAuthenticated, int? userId, string? userRole, Product product)
		{
			if (!isAuthenticated)
			{
				return false;
			}

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


		private bool HandleUpdate(bool isAuthenticated, int? userId, string? userRole, Product product)
		{
			if (!isAuthenticated)
			{
				return false;
			}

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


		private bool HandleGet(bool isAuthenticated, bool isAdult, string? userRole, Product product)
		{
			if (isAuthenticated && (userRole != UserRoles.Buyer))
			{
				return true;
			}

			if ((!isAuthenticated || !isAdult) && product.Category.IsForAdults)
			{
				return false;
			}

			return true;
		}


		private bool HandleAdd(bool isAuthenticated, string? userRole)
		{
			if (isAuthenticated && (userRole != UserRoles.Buyer))
			{
				return true;
			}

			return false;
		}
	}
}
