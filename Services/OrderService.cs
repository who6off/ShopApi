using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopApi.Authentication;
using ShopApi.Authorization;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers.Exceptions;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Order;
using ShopApi.Models.Requests;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		private readonly HttpContext _httpContext;

		public OrderService(
			IOrderRepository orderRepository,
			IProductRepository productRepository,
			IHttpContextAccessor httpContextAccessor,
			IMapper mapper
		)
		{
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_mapper = mapper;

			if (httpContextAccessor.HttpContext is null)
			{
				throw new Exception("Unable to get HttpContext");
			}

			_httpContext = httpContextAccessor.HttpContext;
		}


		public async Task<IPageData<OrderDTO>> Get(OrderSearchParameters searchParameters)
		{
			var userRole = _httpContext.User.GetUserRole();

			if (userRole != UserRoles.Admin)
			{
				var userId = _httpContext.User.GetUserId();

				if (searchParameters.BuyerId is null)
				{
					searchParameters.BuyerId = userId;
				}
				else if ((searchParameters.BuyerId is not null) && (searchParameters.BuyerId != userId))
				{
					throw new AccessDeniedException("Access denied!");
				}
			}

			var data = await _orderRepository.Get(searchParameters);
			var dataMap = data.Map<OrderDTO>(_mapper);
			return dataMap;
		}


		public async Task<OrderDTO> GetById(int id)
		{
			var order = await _orderRepository.GetById(id);

			//Authorization

			if (order is null)
			{
				throw new NotFoundException("Order is not found!");
			}

			var orderDto = _mapper.Map<OrderDTO>(order);
			return orderDto;
		}


		public async Task<OrderDTO> Add(OrderForCreationDTO dto)
		{
			dto.OrderItems = GroupOrderItems(dto.OrderItems);
			var orderedProducts = await _productRepository.GetRangeById(dto.OrderItems!.Select(i => i.ProductId!.Value));
			CheckForProhibitedItems(orderedProducts);

			var order = _mapper.Map<Order>(dto);
			order.BuyerId = _httpContext.User.GetUserId();
			order.Date = DateTime.Now;

			var newOrder = await _orderRepository.Add(order);

			if (newOrder is null)
			{
				throw new AppException("Order creation error!");
			}

			var newOrderDto = _mapper.Map<OrderDTO>(newOrder);
			return newOrderDto;
		}


		public async Task<OrderDTO> Update(int id, OrderForUpdateDTO dto)
		{
			var order = await _orderRepository.GetById(id);

			if (order is null)
			{
				throw new NotFoundException("Order is not found!");
			}

			//Authorization

			dto.OrderItems = GroupOrderItems(dto.OrderItems);
			var orderedProducts = await _productRepository.GetRangeById(dto.OrderItems!.Select(i => i.ProductId!.Value));
			CheckForProhibitedItems(orderedProducts);

			_mapper.Map(dto, order);
			order.Date = DateTime.Now;

			var updatedOrder = await _orderRepository.Update(order);

			if (updatedOrder is null)
			{
				throw new AppException("Order creation error!");
			}

			var updatedOrderDto = _mapper.Map<OrderDTO>(updatedOrder);
			return updatedOrderDto;
		}


		public async Task<OrderDTO> Delete(int id)
		{
			var order = await _orderRepository.GetById(id);

			if (order is null)
			{
				throw new NotFoundException("Order is not found!");
			}

			//Authorization

			var deletedOrder = await _orderRepository.Delete(id);

			if (deletedOrder is null)
			{
				throw new AppException("Delete error!");
			}

			var deletedOrderDto = _mapper.Map<OrderDTO>(deletedOrder);
			return deletedOrderDto;
		}


		public async Task<OrderItemDTO> AddOrderItem(int orderId, OrderItemForCreationDTO dto)
		{
			var order = await _orderRepository.GetById(orderId);

			if (order is null)
			{
				throw new NotFoundException("Order is not found!");
			}

			//Authorization

			var orderItem = _mapper.Map<OrderItem>(dto);
			orderItem.OrderId = orderId;

			var newOrderItem = await _orderRepository.AddOrderItem(orderItem);
			if (newOrderItem is null)
			{
				throw new AppException("Creation error!");
			}

			var newOrderItemDto = _mapper.Map<OrderItemDTO>(newOrderItem);
			return newOrderItemDto;
		}


		public Task<OrderItemDTO> UpdateOrderItem(int orderId, int itemId, OrderItemForUpdateDTO dto)
		{
			throw new NotImplementedException();
		}

		public Task<OrderItemDTO> DeleteOrderItem(int orderId, int itemId)
		{
			throw new NotImplementedException();
		}


		public async Task<Order[]> GetUserOrders(int id)
		{
			var orders = await _orderRepository.GetByUserId(id)
				.OrderByDescending(i => i.Id)
				.ToArrayAsync();

			return orders;
		}


		public async Task<Order[]> GetSellerOrders(int id)
		{
			var orders = await _orderRepository.GetBySellerId(id)
				.OrderByDescending(i => i.Id)
				.ToArrayAsync();

			return orders;
		}


		//public async Task<Order?> Update(OrderUpdateRequest request)
		//{
		//	var order = await _orderRepository.GetById(request.OrderId);

		//	var newItems = GroupOrderItems(request.Items)
		//		.Select(i =>
		//		{
		//			var existedItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == i.ProductId);
		//			if (existedItem is null)
		//			{
		//				return new OrderItem()
		//				{
		//					OrderId = order.Id,
		//					ProductId = i.ProductId,
		//					Amount = i.Amount
		//				};
		//			}
		//			else
		//			{
		//				order.OrderItems.Remove(existedItem);
		//				existedItem.Amount = i.Amount;
		//				return existedItem;
		//			}
		//		})
		//		.ToArray();

		//	await _orderRepository.DeleteOrderItems(order.OrderItems);

		//	order.OrderItems = newItems;
		//	var updated = await _orderRepository.Update(order);

		//	return updated;
		//}


		public async Task<Order?> RequestDelivery(int id)
		{
			var order = await _orderRepository.GetById(id);
			order.IsRequestedForDelivery = true;
			order.Date = DateTime.Now;

			order = await _orderRepository.Update(order);

			return order;
		}


		public async Task<OrderItem?> GetOrderItemById(int id)
		{
			var orderItem = await _orderRepository.GetOrderItemById(id);
			return orderItem;
		}


		public async Task<OrderItem?> AddProductToOrder(OrderProductRequest request, int buyerId)
		{
			var orderId =
				request.OrderId ??
				(await _orderRepository.FindUnrequestedForDeliveryOrder(buyerId))?.Id;

			if (orderId is not null)
			{
				var orderItem = await _orderRepository.FindOrderItem(orderId.Value, request.ProductId);

				if (orderItem is not null)
				{
					orderItem.Amount += request.Amount;
					return await _orderRepository.UpdateOrderItem(orderItem);
				}
			}

			if (orderId is null)
				orderId = (await _orderRepository.Add(new Order()
				{
					BuyerId = buyerId,
					Date = DateTime.Now
				}))?.Id;

			var result = (orderId is null)
				? null
				: await _orderRepository.AddProductToOrder(new OrderItem()
				{
					ProductId = request.ProductId,
					Amount = request.Amount,
					OrderId = orderId.Value,
				});

			return result;
		}


		public async Task<OrderItem?> UpdateProductInOrder(OrderProductUpdateRequest request, OrderItem orderItem)
		{
			var updatedOrderItem = await _orderRepository.UpdateOrderItem(new OrderItem()
			{
				Id = request.Id,
				Amount = request.Amount,
				ProductId = orderItem.ProductId,
				OrderId = orderItem.OrderId
			});

			return updatedOrderItem;
		}

		public async Task<bool> DeleteProductInOrder(OrderItem orderItem)
		{
			var isDeleted = await _orderRepository.DeleteOrderItem(orderItem);

			return isDeleted;
		}


		private OrderItemForCreationDTO[] GroupOrderItems(OrderItemForCreationDTO[] items)
		{
			return items
				.GroupBy(i => i.ProductId!.Value)
				.Select(g => new OrderItemForCreationDTO()
				{
					ProductId = g.Key,
					Amount = g.Sum(i => i.Amount!.Value)
				})
				.ToArray();
		}


		private void CheckForProhibitedItems(Product[] products)
		{
			if (_httpContext.User.IsAdult())
			{
				return;
			}

			var isAnyProhibited = products.Any(i => i.Category?.IsForAdults ?? true);

			if (isAnyProhibited)
			{
				throw new AccessDeniedException("Some of order items are prohibited to buy!");
			}
		}
	}
}
