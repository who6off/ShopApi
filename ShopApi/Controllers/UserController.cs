using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Authentication;
using ShopApi.Authorization;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Models.DTOs.User;
using ShopApi.Models.Requests.User;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(
			IUserService userService
		)
		{
			_userService = userService;
		}


		[HttpPost]
		[AllowAnonymous]
		[Route("registration")]
		public async Task<IActionResult> Register([FromBody] UserForRegistrationDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var registrationResult = await _userService.Register(dto);

			return Ok(registrationResult);
		}


		[HttpPost]
		[AllowAnonymous]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var loginResult = await _userService.Login(request);

			return Ok(loginResult);
		}


		[HttpGet]
		[Authorize]
		[Route("profile")]
		public async Task<IActionResult> Profile()
		{
			var user = await _userService.GetById(User.GetUserId().Value);

			return Ok(user);
		}


		[HttpPut]
		[Authorize]
		[Route("profile")]
		public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var profileUpdateResult = await _userService.UpdateProfile(dto);

			return Ok(profileUpdateResult);
		}


		[HttpGet]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> GetUsers([FromQuery] UserSearchParameters searchParameters)
		{
			var users = await _userService.Get(searchParameters);
			return Ok(users);
		}


		[HttpGet]
		[Authorize(Roles = UserRoles.Admin)]
		[Route("{id:required}")]
		public async Task<IActionResult> GetUserById([FromRoute] int id)
		{
			var users = await _userService.GetById(id);
			return Ok(users);
		}



		[HttpPost]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> AddUser([FromBody] UserForCreationDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var newUser = await _userService.Add(dto);

			return Ok(newUser);
		}



		[HttpPut]
		[Authorize(Roles = UserRoles.Admin)]
		[Route("{id:required}")]
		public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserForUpdateDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var updatedUser = await _userService.Update(id, dto);

			return Ok(updatedUser);
		}


		[HttpDelete]
		[Authorize(Roles = UserRoles.Admin)]
		[Route("{id:required}")]
		public async Task<IActionResult> DeleteUser([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var deletedUser = await _userService.Delete(id);

			return Ok(deletedUser);
		}


		[HttpGet]
		[Authorize(Roles = UserRoles.Admin)]
		[Route("roles")]
		public async Task<IActionResult> GetRoles()
		{
			var roles = await _userService.GetRoles();
			return Ok(roles);
		}
	}
}
