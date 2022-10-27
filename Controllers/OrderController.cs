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
        private readonly IConfiguration _configuration;

        public OrderController(
            IConfiguration configuration,
            IOrderService orderService,
            IProductService productService)
        {
            _configuration = configuration;
            _orderService = orderService;
            _productService = productService;
        }


        [HttpGet]
        [Route("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserOrders(int id)
        {
            var userId = HttpContext.User.GetUserId();
            var userRole = HttpContext.User.GetUserRole();

            if (userRole != UserRoles.Admin && userId != id)
                return StatusCode(StatusCodes.Status403Forbidden);

            var orders = await _orderService.GetUserOrders(id);

            return (orders is null) ? BadRequest() : Ok(orders);
        }


        [HttpGet]
        [Route("seller/{id}")]
        [Authorize]
        public async Task<IActionResult> GetSellerOrders(int id)
        {
            var userId = HttpContext.User.GetUserId();
            var userRole = HttpContext.User.GetUserRole();

            if (userRole != UserRoles.Admin && userId != id)
                return StatusCode(StatusCodes.Status403Forbidden);

            var orders = await _orderService.GetSellerOrders(id);

            return (orders is null) ? BadRequest() : Ok(orders);
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
        public async Task<IActionResult> CreateNewOrder(OrderCreationRequest? request = null)
        {
            if (request is not null)
                request.Items = await GetPermitedRequestItems(request.Items);

            var newOrder = await _orderService.Add(new Order()
            {
                BuyerId = HttpContext.User.GetUserId().Value,
                Date = DateTime.Now,
            }, request);

            return (newOrder is null) ? BadRequest() : Ok(newOrder);
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(OrderUpdateRequest request)
        {
            if (!(await IsPermitedOrder(request.OrderId)))
                return StatusCode(StatusCodes.Status403Forbidden);

            request.Items = await GetPermitedRequestItems(request.Items);

            var updatedOrder = await _orderService.Update(request);

            return (updatedOrder is null) ? BadRequest() : Ok(updatedOrder);
        }


        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> RequestDelivery(int id)
        {
            if (!(await IsPermitedOrder(id)))
                return StatusCode(StatusCodes.Status403Forbidden);

            var order = await _orderService.RequestDelivery(id);

            return (order is null) ? BadRequest() : Ok(order);
        }


        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!(await IsPermitedOrder(id)))
                return StatusCode(StatusCodes.Status403Forbidden);

            var idDeleted = await _orderService.Delete(id);

            return (idDeleted) ? Ok() : BadRequest();
        }


        [HttpPost]
        [Route("item")]
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
        [Route("item")]
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

        [HttpDelete]
        [Route("item/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _orderService.GetOrderItemById(id);

            if (orderItem is null)
                return StatusCode(StatusCodes.Status404NotFound);

            if (!(await IsPermitedOrder(orderItem.OrderId.Value)))
                return StatusCode(StatusCodes.Status403Forbidden);

            var isDeleted = await _orderService.DeleteProductInOrder(orderItem);
            return isDeleted ? Ok() : BadRequest();
        }

        private async Task<bool> IsPermitedOrder(int orderId)
        {
            var buyerId = HttpContext.User.GetUserId();
            return await IsPermitedOrder(orderId, buyerId.Value);
        }

        private async Task<bool> IsPermitedOrder(int orderId, int buyerId)
        {
            var order = await _orderService.GetById(orderId);
            if (order is null)
                return false;

            return (order.BuyerId == buyerId) && !order.IsRequestedForDelivery;
        }

        private async Task<OrderRequestItem[]> GetPermitedRequestItems(OrderRequestItem[] items)
        {
            var age = HttpContext.User.GetUserAge();
            var adultAge = _configuration.GetAdultAge();
            Predicate<Product?> Predicate = (age < adultAge)
                ? (Product? product) => product is not null && !product.Category.IsForAdults
                : (Product? product) => product is not null;

            return await Task.Run(() =>
            {
                return items
                .Where((i) =>
                {
                    var product = _productService.GetById(i.ProductId).Result;
                    return Predicate(product);
                })
                .ToArray();
            });
        }
    }
}
