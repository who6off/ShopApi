using HelloApi.Models;
using HelloApi.Models.Requests;

namespace HelloApi.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Order?> GetById(int id);
        public Task<Order?> Add(Order order);
        public Task<bool> Delete(int id);

        public Task<Order?> RequestDelivery(int id);


        public Task<OrderItem?> GetOrderItemById(int id);
        public Task<OrderItem?> AddProductToOrder(OrderProductRequest request, int buyerId);
        public Task<OrderItem?> UpdateProductInOrder(OrderProductUpdateRequest request, OrderItem orderItem);
        public Task<bool> DeleteProductInOrder(OrderItem orderItem);
    }
}
