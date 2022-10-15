﻿using HelloApi.Authorization;
using HelloApi.Services;
using Microsoft.AspNetCore.Mvc;

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
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var token = _userService.Login(loginRequest);
                return (token is null) ? NotFound() : Ok(new { Token = token.Result });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userService.GetAllRoles();
            return Ok(roles);
        }
    }
}