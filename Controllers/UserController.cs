using HelloApi.Authentication;
using HelloApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelloApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            try
            {
                var registrationResponce = await _userService.Register(request);

                if (registrationResponce == null)
                    return BadRequest();

                return Ok(registrationResponce);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var token = await _userService.Login(loginRequest);
                return (token is null) ? NotFound() : Ok(new { Token = token });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            var role = HttpContext.User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Role).Value;
            Console.WriteLine(role);
            var users = await _userService.GetAll();
            return Ok(new object[] { role, users });
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
