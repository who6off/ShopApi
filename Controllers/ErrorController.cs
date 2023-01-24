using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ErrorController : ControllerBase
	{

		[HttpGet]
		[Route("dev")]
		public IActionResult Development([FromServices] IWebHostEnvironment webHostEnvironment)
		{
			if (webHostEnvironment.EnvironmentName != "Development")
			{
				throw new InvalidOperationException(
					"This shouldn't be invoked in non-development environments.");
			}

			var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

			return Problem(
				title: context?.Error.Message,
				detail: context?.Error.StackTrace);
		}


		[HttpGet]
		[Route("prod")]
		public IActionResult Production()
		{
			var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

			return Problem(title: context?.Error.Message);
		}
	}
}
