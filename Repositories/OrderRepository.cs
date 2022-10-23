using HelloApi.Data;
using HelloApi.Models;
using HelloApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class OrderRepository : ARepository<ShopContext>, IOrderRepository
    {
        public OrderRepository(ShopContext context) : base(context) { }

        public async Task<Order?> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Id == id)
                .FirstAsync();
            return order;
        }
        public async Task<Order> Add(Order order)
        {
            var newOrder = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return newOrder.Entity;
        }

        public async Task<Order?> FindUnrequestedForDeliveryOrder(int buyerId)
        {
            try
            {
                var order = await _context
                    .Orders
                    .FirstOrDefaultAsync(i => i.BuyerId == buyerId && !i.IsRequestedForDelivery);
                return order;
            }
            catch (Exception e)
            {
                //TODO: Add Log!
                return null;
            }
        }

        public async Task<OrderItem?> AddProductToOrder(int orderId, int productId, uint amount = 1)
        {
            try
            {
                var orderItem = await _context.AddAsync<OrderItem>(new OrderItem()
                {
                    OrderId = orderId,
                    ProductId = productId,
                    Amount = amount
                });
                await _context.SaveChangesAsync();

                return orderItem.Entity;
            }
            catch
            {
                //TODO: Add log
                return null;
            }
        }
    }
}
