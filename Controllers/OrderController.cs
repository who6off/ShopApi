using HelloApi.Authentication;
using HelloApi.Authorization;
using HelloApi.Configuration;
using HelloApi.Models;
using HelloApi.Models.Requests;
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
        private readonly IProductService _productService;

        public OrderController(
            IOrderService orderService,
            IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = HttpContext.User.GetUserId();
            var userRole = HttpContext.User.GetUserRole();

            var order = await _orderService.GetById(id);

            if (userRole != UserRoles.Admin && userId != order.BuyerId)
                return StatusCode(StatusCodes.Status403Forbidden);

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


        [HttpPost]
        [Route("product")]
        [Authorize]
        public async Task<IActionResult> AddProductToOrder(
            OrderProductRequest request,
            [FromServices] IConfiguration configuration
            )
        {
            var buyerId = HttpContext.User.GetUserId();
            var product = await _productService.GetById(request.ProductId);

            if (product is null)
                return StatusCode(StatusCodes.Status404NotFound);

            if (product.Category.IsForAdults)
            {
                var buyerAge = HttpContext.User.GetUserAge();
                var adultAge = configuration.GetAdultAge();

                if (buyerAge < adultAge)
                    return StatusCode(StatusCodes.Status403Forbidden);
            }

            if (request.OrderId is not null)
            {
                if (!(await IsPermitedOrder(request.OrderId.Value)))
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }

            var orderItem = await _orderService.AddProductToOrder(request, buyerId.Value);

            return (orderItem is null) ? BadRequest() : Ok(orderItem);
        }

        [HttpPut]
        [Route("product")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderItem(OrderProductUpdateRequest request)
        {
            var orderItem = await _orderService.GetOrderItemById(request.Id);

            if (orderItem is null)
                return StatusCode(StatusCodes.Status404NotFound);

            if (!(await IsPermitedOrder(orderItem.OrderId.Value)))
                return StatusCode(StatusCodes.Status403Forbidden);

            var result = await _orderService.UpdateProductInOrder(request, orderItem);
            return (result is null) ? BadRequest() : Ok(result);
        }
        private async Task<bool> IsPermitedOrder(int orderId)
        {
            var buyerId = HttpContext.User.GetUserId();
            return await IsPermitedOrder(orderId, buyerId.Value);
        }

        private async Task<bool> IsPermitedOrder(int orderId, int buyerId)
        {
            var storedId = (await _orderService.GetById(orderId))?.BuyerId ?? 0;
            return storedId == buyerId;
        }
    }
}
