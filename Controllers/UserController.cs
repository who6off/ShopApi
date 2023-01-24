using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Authentication;
using ShopApi.Authorization;
using ShopApi.Data.Models;
using ShopApi.Models.DTOs.User;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public UserController(
			IUserService userService,
			IMapper mapper
		)
		{
			_userService = userService;
			_mapper = mapper;
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register(UserForCreationDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = _mapper.Map<User>(dto);
			var registrationResult = await _userService.Register(user, dto.Password);

			if (registrationResult is null)
			{
				return BadRequest();
			}

			var resultDto = _mapper.Map<UserRegistrationResultDTO>(registrationResult);
			return Ok(resultDto);
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
			var user = await _userService.GetById(HttpContext.User.GetUserId().Value);
			return (user is null) ? NotFound() : Ok(user);
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
