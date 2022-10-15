using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public ProductsController()
        {

        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok(new { Message = "Test" });
        }

        [HttpGet]
        [Route("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok(new { Message = "Test Auth" });
        }
    }
}
