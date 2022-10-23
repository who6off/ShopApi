using HelloApi.Models;

namespace HelloApi.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Order?> Add(Order order);
        public Task<Order?> GetById(int id);

    }
}
