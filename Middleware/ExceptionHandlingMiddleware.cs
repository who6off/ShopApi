using ShopApi.Helpers.Exceptions;

namespace ShopApi.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}


		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}

			catch (AppException e)
			{
				HandleException(e, httpContext);
			}
			catch (Exception e)
			{
				HandleException(new AppException(), httpContext);
			}
		}


		private async void HandleException(AppException exception, HttpContext httpContext)
		{
			httpContext.Response.StatusCode = exception.ErrorCode;
			httpContext.Response.ContentType = "application/json";

			await httpContext.Response.WriteAsJsonAsync(new
			{
				ErrorCode = exception.ErrorCode,
				Message = exception.Message
			});
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class ExceptionHandlingMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandlingMiddleware>();
		}
	}
}
