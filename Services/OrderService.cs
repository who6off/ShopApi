using HelloApi.Models;
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
        public async Task<Order?> Add(Order order)
        {
            var existingOrder = await _orderRepository.FindUnrequestedForDeliveryOrder(order.BuyerId);
            if (existingOrder != null)
                return null;

            var newOrder = await _orderRepository.Add(order);
            return newOrder;
        }

        public async Task<Order?> GetById(int id)
        {
            return await _orderRepository.GetById(id);
        }
    }
}
