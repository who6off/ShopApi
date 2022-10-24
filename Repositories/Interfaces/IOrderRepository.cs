using HelloApi.Models;

namespace HelloApi.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order?> GetById(int id);
        public Task<Order?> Add(Order order);
        public Task<Order?> FindUnrequestedForDeliveryOrder(int buyerId);

        public Task<OrderItem> AddProductToOrder(OrderItem orderItem);
    }
}
