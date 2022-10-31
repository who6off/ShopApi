namespace HelloApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                //TODO: Add log
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (_environment.IsDevelopment())
                {
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        Message = e.Message
                    });
                }
                else
                {
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        Message = "Sorry, something went wrong. Try again later"
                    });
                }
            }
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
