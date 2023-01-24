using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Authentication;
using ShopApi.Authorization;
using ShopApi.Models.Requests;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register(RegistrationRequest request)
		{

			var registrationResponce = await _userService.Register(request);

			if (registrationResponce == null)
				return BadRequest();

			return Ok(registrationResponce);

		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login(LoginRequest loginRequest)
		{

			var token = await _userService.Login(loginRequest);
			return (token is null) ? NotFound() : Ok(new { Token = token });
		}

		[HttpPost]
		[Route("profile")]
		[Authorize]
		public async Task<IActionResult> Profile()
		{
			var token = await _userService.GetById(HttpContext.User.GetUserId().Value);
			return (token is null) ? NotFound() : Ok(new { Token = token });
		}

		[HttpGet]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _userService.GetAll();
			return Ok(users);
		}

		[HttpGet]
		[Route("roles")]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> GetAllRoles()
		{
			var roles = await _userService.GetAllRoles();
			return Ok(roles);
		}
	}
}
