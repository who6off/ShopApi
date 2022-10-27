using HelloApi.Models;

namespace HelloApi.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order?> GetById(int id);
        public IQueryable<Order> GetByUserId(int id);
        public IQueryable<Order> GetBySellerId(int id);
        public Task<Order?> Add(Order order);
        public Task<Order?> Update(Order order);
        public Task<bool> Delete(int id);
        public Task<Order?> FindUnrequestedForDeliveryOrder(int buyerId);

        public Task<OrderItem?> GetOrderItemById(int id);
        public Task<OrderItem?> FindOrderItem(int orderId, int productId);
        public Task<OrderItem?> AddProductToOrder(OrderItem orderItem);
        public Task AddProductsToOrder(ICollection<OrderItem> orderItem);
        public Task<OrderItem?> UpdateOrderItem(OrderItem orderItem);
        public Task<bool> DeleteOrderItem(OrderItem orderItem);
        public Task DeleteOrderItems(ICollection<OrderItem> orderItems);
    }
}
