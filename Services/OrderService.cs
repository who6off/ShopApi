using HelloApi.Models;
using HelloApi.Models.Requests;
using HelloApi.Repositories.Interfaces;
using HelloApi.Services.Interfaces;

namespace HelloApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> GetById(int id)
        {
            return await _orderRepository.GetById(id);
        }
        public async Task<Order?> Add(Order order)
        {
            var existingOrder = await _orderRepository.FindUnrequestedForDeliveryOrder(order.BuyerId);
            if (existingOrder != null)
                return null;

            var newOrder = await _orderRepository.Add(order);
            return newOrder;
        }

        public async Task<OrderItem?> AddProductToOrder(OrderProductRequest request, int buyerId)
        {
            var orderId =
                request.OrderId ??
                (await _orderRepository.FindUnrequestedForDeliveryOrder(buyerId))?.Id;

            if (orderId is null)
                orderId = (await _orderRepository.Add(new Order()
                {
                    BuyerId = buyerId,
                    Date = DateTime.Now
                }))?.Id;

            if (orderId is null)
                return null;

            var orderItem = await _orderRepository.AddProductToOrder(new OrderItem()
            {
                ProductId = request.ProductId,
                Amount = request.Amount,
                OrderId = orderId.Value,
            });

            return orderItem;
        }

    }
}
