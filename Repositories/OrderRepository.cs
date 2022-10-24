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
        public async Task<Order?> Add(Order order)
        {
            var newOrder = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return newOrder?.Entity;
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

        public async Task<OrderItem?> AddProductToOrder(OrderItem orderItem)
        {
            try
            {
                var newOrderItem = await _context.AddAsync<OrderItem>(orderItem);
                await _context.SaveChangesAsync();

                return newOrderItem.Entity;
            }
            catch
            {
                //TODO: Add log
                return null;
            }
        }

        public async Task<OrderItem?> FindOrderItem(int orderId, int productId)
        {
            try
            {
                var orderItem = await _context
                    .Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.Id == orderId)
                    .Select(o => o.OrderItems.Where(oi => oi.ProductId == productId))
                    .FirstOrDefaultAsync();

                return orderItem?.First();
            }
            catch (Exception e)
            {
                //TODO: Add log
                return null;
            }
        }

        public async Task<OrderItem?> GetOrderItemById(int id)
        {
            try
            {
                var orderItem = await _context
                    .Orders
                    .Where(o => o.OrderItems.Any(oi => oi.Id == id))
                    .Select(o => o.OrderItems.Where(oi => oi.Id == id))
                    .FirstOrDefaultAsync();

                return orderItem?.First();
            }
            catch (Exception e)
            {
                //TODO: Add log
                return null;
            }
        }


        public Task<OrderItem?> UpdateOrderItem(OrderItem orderItem)
        {
            return Task.Run(() =>
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    var updetedOrderItem = _context.Update<OrderItem>(orderItem);
                    _context.SaveChanges();
                    return updetedOrderItem.Entity;
                }
                catch (Exception e)
                {
                    //TODO: Add log
                    return null;
                }
            });
        }
    }
}
