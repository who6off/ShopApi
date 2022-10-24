using HelloApi.Models;
using HelloApi.Models.Requests;

namespace HelloApi.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Order?> GetById(int id);
        public Task<Order?> Add(Order order);

        public Task<OrderItem?> AddProductToOrder(OrderProductRequest request, int buyerId);
    }
}
