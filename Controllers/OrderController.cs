using HelloApi.Authorization;
using HelloApi.Models;
using HelloApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetById(id);

            return (order is null) ? BadRequest() : Ok(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewOrder()
        {
            var newOrder = await _orderService.Add(new Order()
            {
                BuyerId = HttpContext.User.GetUserId().Value,
                Date = DateTime.Now,
            });

            return (newOrder is null) ? BadRequest() : Ok(newOrder);
        }
    }
}
