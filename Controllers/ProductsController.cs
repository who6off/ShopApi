using HelloApi.Authorization;
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
        [Authorize]
        public IActionResult Index()
        {
            return Ok(new { Message = "Products" });
        }

        [HttpGet]
        [Route("adults")]
        [Authorize(Policy = AgeRestrictionPolicy.Name)]
        public IActionResult Adult()
        {
            return Ok(new { Message = "Adult Products" });
        }
    }
}
