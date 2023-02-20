using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ShopApi.Authentication;
using ShopApi.Authorization;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers.Exceptions;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Order;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly IAuthorizationService _authorizationService;
		private readonly IMapper _mapper;
		private readonly HttpContext _httpContext;

		public OrderService(
			IOrderRepository orderRepository,
			IProductRepository productRepository,
			IHttpContextAccessor httpContextAccessor,
			IAuthorizationService authorizationService,
			IMapper mapper
		)
		{
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_authorizationService = authorizationService;
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
			var userId = _httpContext.User.GetUserId();

			if (userRole == UserRoles.Buyer)
			{
				if (searchParameters.BuyerId is null)
				{
					searchParameters.BuyerId = userId;
				}
				else if ((searchParameters.BuyerId is not null) && (searchParameters.BuyerId != userId))
				{
					throw new AccessDeniedException("Access denied!");
				}
			}
			else if (userRole == UserRoles.Seller)
			{
				if ((searchParameters.BuyerId is null) && (searchParameters.SellerId is null))
				{
					throw new ClientInputException("Not enough parameters!");
				}

				if ((searchParameters.BuyerId != userId) && (searchParameters.SellerId != userId))
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
			await Authorize(order, OrderOperations.Get);

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
			//Set PurchasePrice

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
			await Authorize(order, OrderOperations.Update);

			dto.OrderItems = GroupOrderItems(dto.OrderItems);
			var orderedProducts = await _productRepository.GetRangeById(dto.OrderItems!.Select(i => i.ProductId!.Value));
			CheckForProhibitedItems(orderedProducts);

			_mapper.Map(dto, order);
			order.Date = DateTime.Now;
			//Set PurchasePrice

			var updatedOrder = await _orderRepository.Update(order);

			if (updatedOrder is null)
			{
				throw new AppException("Order update error!");
			}

			var updatedOrderDto = _mapper.Map<OrderDTO>(updatedOrder);
			return updatedOrderDto;
		}


		public async Task<OrderDTO> Delete(int id)
		{
			var order = await _orderRepository.GetById(id);
			await Authorize(order, OrderOperations.Delete);

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
			await Authorize(order, OrderOperations.Update);

			var product = await _productRepository.GetById(dto.ProductId.Value);
			CheckForProhibitedItems(product);

			OrderItem? newOrderItem = null;
			var sameExistingItem = order.OrderItems.FirstOrDefault(i => i.ProductId == dto.ProductId);

			if (sameExistingItem is null)
			{
				var orderItem = _mapper.Map<OrderItem>(dto);
				orderItem.OrderId = orderId;
				newOrderItem = await _orderRepository.AddOrderItem(orderItem);
			}
			else
			{
				sameExistingItem.Amount += (uint)dto.Amount;
				newOrderItem = await _orderRepository.UpdateOrderItem(sameExistingItem);
			}

			if (newOrderItem is null)
			{
				throw new AppException("Item creation error!");
			}

			var newOrderItemDto = _mapper.Map<OrderItemDTO>(newOrderItem);
			return newOrderItemDto;
		}


		public async Task<OrderItemDTO> UpdateOrderItem(int orderId, int itemId, OrderItemForUpdateDTO dto)
		{
			var order = await _orderRepository.GetById(orderId);
			var orderItem = order?.OrderItems.FirstOrDefault(i => i.Id == itemId);
			await Authorize(order, orderItem, OrderOperations.Update);

			var product = await _productRepository.GetById(dto.ProductId.Value);
			CheckForProhibitedItems(product);

			orderItem = _mapper.Map<OrderItem>(dto);
			orderItem.Id = itemId;
			orderItem.OrderId = orderId;

			var updatedOrderItem = await _orderRepository.UpdateOrderItem(orderItem);
			if (updatedOrderItem is null)
			{
				throw new AppException("Item update error!");
			}

			var updatedOrderItemDto = _mapper.Map<OrderItemDTO>(updatedOrderItem);
			return updatedOrderItemDto;
		}


		public async Task<OrderItemDTO> DeleteOrderItem(int orderId, int itemId)
		{
			var order = await _orderRepository.GetById(orderId);
			var orderItem = order?.OrderItems.FirstOrDefault(i => i.Id == itemId);
			await Authorize(order, orderItem, OrderOperations.Update);

			OrderItem? deletedOrderItem = null;

			if (order.OrderItems.Count == 1)
			{
				var deletedOrder = await _orderRepository.Delete(order.Id);
				if (deletedOrder is null)
				{
					throw new AppException("Delete error!");
				}

				deletedOrderItem = order.OrderItems.ElementAt(0);
			}
			else
			{
				deletedOrderItem = await _orderRepository.DeleteOrderItem(itemId);
			}


			if (deletedOrderItem is null)
			{
				throw new AppException("Item delete error!");
			}

			var deletedOrderItemDto = _mapper.Map<OrderItemDTO>(deletedOrderItem);
			return deletedOrderItemDto;
		}


		public async Task<OrderDTO> RequestDelivery(int id)
		{
			var order = await _orderRepository.GetById(id);
			await Authorize(order, OrderOperations.DeliveryRequest);

			if (order.IsCanceled || order.IsDelivered)
			{
				throw new ClientInputException("Order is completed!");
			}

			if (order.IsRequestedForDelivery)
			{
				throw new ClientInputException("Order is already requested for delivery!");
			}

			order.IsRequestedForDelivery = true;
			order.Date = DateTime.Now;
			//Set PurcasePrice

			var updatedOrder = await _orderRepository.Update(order);

			if (updatedOrder is null)
			{
				throw new AppException("Order creation error!");
			}

			var updatedOrderDto = _mapper.Map<OrderDTO>(updatedOrder);
			return updatedOrderDto;
		}


		public async Task<OrderDTO> CancelOrder(int id)
		{
			var order = await _orderRepository.GetById(id);
			await Authorize(order, OrderOperations.Cancellation);

			if (!order.IsRequestedForDelivery)
			{
				throw new ClientInputException("Can`t cancel unfinished order!");
			}

			if (order.IsCanceled || order.IsDelivered)
			{
				throw new ClientInputException("Order is completed!");
			}

			order.IsCanceled = true;
			var updatedOrder = await _orderRepository.Update(order);

			if (updatedOrder is null)
			{
				throw new AppException("Order creation error!");
			}

			var updatedOrderDto = _mapper.Map<OrderDTO>(updatedOrder);
			return updatedOrderDto;
		}


		private async Task Authorize(Order? order, IAuthorizationRequirement requirement)
		{
			if (order is null)
			{
				throw new NotFoundException("Order is not found!");
			}

			var isAuthorized = await _authorizationService.AuthorizeAsync(_httpContext.User, order, requirement);

			if (!isAuthorized.Succeeded)
			{
				throw new AccessDeniedException("Access denied!");
			}

			if ((requirement == OrderOperations.Update) && order.IsRequestedForDelivery)
			{
				throw new ClientInputException("Can't change completed order!");
			}

			if (
				(requirement == OrderOperations.Delete) &&
				order.IsRequestedForDelivery &&
				(_httpContext.User.GetUserRole() != UserRoles.Admin)
			)
			{
				throw new ClientInputException("Can't delete completed order!");
			}
		}


		private async Task Authorize(Order? order, OrderItem? orderItem, IAuthorizationRequirement requirement)
		{
			await Authorize(order, requirement);

			if (orderItem is null)
			{
				throw new NotFoundException("Order item is not found");
			}
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


		private void CheckForProhibitedItems(params Product[] products)
		{
			if ((products is null) || products.Any(i => i is null))
			{
				throw new NotFoundException("Product is not found!");
			}

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
