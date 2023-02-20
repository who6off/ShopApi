using Microsoft.EntityFrameworkCore;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Data.Repositories
{
	public class OrderRepository : ARepository<ShopContext>, IOrderRepository
	{
		public OrderRepository(ShopContext context) : base(context) { }


		public async Task<IPageData<Order>> Get(OrderSearchParameters searchParameters)
		{
			//var query = _context.Orders
			//	.Include(o => o.OrderItems)
			//		.ThenInclude(oi => oi.Product)
			//			.ThenInclude(p => p.Category)
			//	.Include(o => o.OrderItems)
			//		.ThenInclude(oi => oi.Product)
			//			.ThenInclude(p => p.Seller)
			//	.OrderByDescending(i => i.Date)
			//	.AsQueryable();

			IQueryable<Order> query;

			if (searchParameters.SellerId is not null)
			{
				query = _context.Orders
					.Include(o => o.OrderItems.Where(oi => oi.Product.SellerId == (searchParameters.SellerId)))
					.Where(o => (o.OrderItems.Count > 0) && o.IsRequestedForDelivery);
			}
			else
			{
				query = _context.Orders.Include(o => o.OrderItems);
			}

			query = query
				.Include("OrderItems.Product")
				.Include("OrderItems.Product.Seller")
				.OrderByDescending(i => i.Date)
				.AsQueryable();

			if (searchParameters.BuyerId is not null)
			{
				query = query.Where(i => i.BuyerId == searchParameters.BuyerId);
			}

			if (searchParameters.DateFrom is not null)
			{
				query = query.Where(i => i.Date >= searchParameters.DateFrom);
			}

			if (searchParameters.DateTo is not null)
			{
				query = query.Where(i => i.Date <= searchParameters.DateTo);
			}

			if (searchParameters.Date is not null)
			{
				query = query.Where(i => i.Date == searchParameters.Date);
			}

			if (searchParameters.IsRequestedForDelivery is not null)
			{
				query = query.Where(i => i.IsRequestedForDelivery == searchParameters.IsRequestedForDelivery);
			}

			if (searchParameters.IsCanceled is not null)
			{
				query = query.Where(i => i.IsCanceled == searchParameters.IsCanceled);
			}

			if (searchParameters.IsDelivered is not null)
			{
				query = query.Where(i => i.IsDelivered == searchParameters.IsDelivered);
			}

			if (searchParameters.DeliveryDate is not null)
			{
				query = query.Where(i => i.DeliveryDate == searchParameters.DeliveryDate);
			}

			var totalAmount = await query.CountAsync();
			var data = await query
				.Skip(searchParameters.GetSkip())
				.Take(searchParameters.PageSize)
				.ToArrayAsync();

			var pageData = new PageData<Order>(data, searchParameters.Page, searchParameters.PageSize, totalAmount);
			return pageData;
		}


		public async Task<Order?> GetById(int id)
		{
			try
			{
				var order = await _context.Orders
					.Include(o => o.OrderItems)
						.ThenInclude(oi => oi.Product)
							.ThenInclude(p => p.Category)
					.Include(o => o.OrderItems)
						.ThenInclude(oi => oi.Product)
							.ThenInclude(p => p.Seller)
					.Where(o => o.Id == id)
					.FirstOrDefaultAsync();

				return order;
			}
			catch (Exception e)
			{
				//TODO: Add Log!
				return null;
			}
		}

		public IQueryable<Order> GetByUserId(int id)
		{
			var orders = _context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.Where(o => o.BuyerId == id);

			return orders;
		}


		public IQueryable<Order> GetBySellerId(int id)
		{
			var orders = _context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.Where(o => o.IsRequestedForDelivery && o.OrderItems.Any(oi => oi.Product.SellerId == id))
				.Select(o => new Order()
				{
					Id = o.Id,
					BuyerId = o.BuyerId,
					Date = o.Date,
					IsRequestedForDelivery = o.IsRequestedForDelivery,
					OrderItems = o.OrderItems.Where(oi => oi.Product.SellerId == id).ToArray(),
				});

			return orders;
		}


		public async Task<Order?> Add(Order order)
		{
			try
			{
				var newOrderEntry = await _context.Orders.AddAsync(order);
				await _context.SaveChangesAsync();

				var newOrder = newOrderEntry.Entity;

				//Not necessary if products loaded for prohibition checking in a service before creating order.
				//
				//await _context.Entry(newOrder)
				//	.Collection(i => i.OrderItems)
				//	.Query()
				//	.Include(i => i.Product)
				//	.Include(i => i.Product.Category)
				//	.Include(i => i.Product.Seller)
				//	.LoadAsync();

				return newOrder;

			}
			catch (Exception e)
			{
				return null;
			}
		}


		public Task<Order?> Update(Order order)
		{
			return Task.Run(() =>
			{
				try
				{
					_context.ChangeTracker.Clear();

					var previousOrderItems = _context.Orders.AsNoTracking()
						.Where(i => i.Id == order.Id)
						.Include(i => i.OrderItems)
						.Select(i => i.OrderItems)
						.FirstOrDefault()
						.Where(i =>
						{
							var newOrderItem = order.OrderItems.FirstOrDefault(j => j.ProductId == i.ProductId);

							if (newOrderItem is null)
							{
								return true;
							}
							else
							{
								newOrderItem.Id = i.Id;
							}

							return false;
						});

					_context.RemoveRange(previousOrderItems);
					var entityEntry = _context.Orders.Update(order);
					_context.SaveChanges();

					var updatedOrder = entityEntry.Entity;
					_context.Entry(updatedOrder)
						.Collection(i => i.OrderItems)
						.Query()
						.Include(i => i.Product)
						.Include(i => i.Product.Category)
						.Include(i => i.Product.Seller)
						.Load();

					return updatedOrder;
				}
				catch (Exception e)
				{
					//TODO: Add Log!
					return null;
				}
			});
		}


		public Task<Order?> Delete(int id)
		{
			return Task.Run(async () =>
			{
				var order = await GetById(id);

				if (order is null)
				{
					return null;
				}

				try
				{
					_context.ChangeTracker.Clear();
					var entityEntry = _context.Orders.Remove(order);
					_context.SaveChanges();
					return (entityEntry is null) ? null : order;
				}
				catch (Exception)
				{
					//TODO: Add Log!
					return null;
				}
			});
		}


		public async Task<OrderItem?> GetOrderItemById(int id)
		{
			try
			{
				var orderItem = await _context.OrderItem
					.Include(i => i.Product)
					.Include(i => i.Product.Category)
					.Include(i => i.Product.Seller)
					.FirstOrDefaultAsync(i => i.Id == id);

				return orderItem;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public async Task<OrderItem?> AddOrderItem(OrderItem orderItem)
		{
			try
			{
				var entityEntry = await _context.OrderItem.AddAsync(orderItem);
				await _context.SaveChangesAsync();

				var newOrderItem = entityEntry?.Entity;
				if (newOrderItem is null)
				{
					return null;
				}

				await _context.Entry(newOrderItem)
					.Reference(i => i.Product)
					.Query()
					.Include(i => i.Category)
					.Include(i => i.Seller)
					.LoadAsync();

				return newOrderItem;
			}
			catch (Exception e)
			{
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
					var entityEntry = _context.OrderItem.Update(orderItem);
					_context.SaveChanges();

					var updatedOrderItem = entityEntry?.Entity;
					if (updatedOrderItem is null)
					{
						return null;
					}

					_context.Entry(updatedOrderItem)
						.Reference(i => i.Product)
						.Query()
						.Include(i => i.Category)
						.Include(i => i.Seller)
						.Load();

					return updatedOrderItem;
				}
				catch (Exception e)
				{
					return null;
				}
			});
		}


		public Task<OrderItem?> DeleteOrderItem(int id)
		{
			return Task.Run(async () =>
			{
				try
				{
					var orderItem = await GetOrderItemById(id);
					if (orderItem is null)
					{
						return null;
					}

					_context.ChangeTracker.Clear();
					var entityEntry = _context.OrderItem.Remove(orderItem);
					_context.SaveChanges();

					return (entityEntry is null) ? null : orderItem;
				}
				catch (Exception e)
				{
					return null;
				}
			});
		}
	}
}


