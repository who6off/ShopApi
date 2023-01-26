using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Authentication;
using ShopApi.Authorization;
using ShopApi.Data.Models;
using ShopApi.Models.DTOs.Role;
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
		[AllowAnonymous]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] UserForCreationDTO dto)
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
		[AllowAnonymous]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var loginResult = await _userService.Login(dto.Email, dto.Password);

			if (loginResult is null)
			{
				throw new Exception("Incorrect user credentials");
			}

			var loginResultDto = _mapper.Map<UserLoginResultDTO>(loginResult);
			return Ok(loginResultDto);
		}


		[HttpGet]
		[Authorize]
		[Route("profile")]
		public async Task<IActionResult> Profile()
		{
			var headers = Request.Headers;
			var user = await _userService.GetById(User.GetUserId().Value);
			return (user is null)
				? NotFound()
				: Ok(_mapper.Map<UserDTO>(user));
		}


		[HttpGet]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _userService.GetAll();
			var usersMap = _mapper.Map<UserDTO[]>(users);
			return Ok(usersMap);
		}


		[HttpGet]
		[Authorize(Roles = UserRoles.Admin)]
		[Route("roles")]
		public async Task<IActionResult> GetAllRoles()
		{
			var roles = await _userService.GetAllRoles();
			var rolesMap = _mapper.Map<RoleDTO[]>(roles);
			return Ok(rolesMap);
		}
	}
}
