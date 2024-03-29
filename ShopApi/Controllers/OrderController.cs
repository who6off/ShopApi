﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Models.DTOs.Order;
using ShopApi.Models.Requests.Order;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController(
			IOrderService orderService
		)
		{
			_orderService = orderService;
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetOrders([FromQuery] OrderSearchParameters searchParameters)
		{
			var orders = await _orderService.Get(searchParameters);

			return Ok(orders);
		}


		[HttpGet]
		[Authorize]
		[Route("{id:required}")]
		public async Task<IActionResult> GetOrderById([FromRoute] int id)
		{
			var order = await _orderService.GetById(id);

			return Ok(order);
		}


		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddOrder([FromBody] OrderForCreationDTO? dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var newOrder = await _orderService.Add(dto);

			return Ok(newOrder);
		}


		[HttpPut]
		[Authorize]
		[Route("{id:required}")]
		public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderForUpdateDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var updatedOrder = await _orderService.Update(id, dto);

			return Ok(updatedOrder);
		}


		[HttpDelete]
		[Authorize]
		[Route("{id:required}")]
		public async Task<IActionResult> DeleteOrder([FromRoute] int id)
		{
			var deletedOrder = await _orderService.Delete(id);

			return Ok(deletedOrder);
		}


		[HttpPost]
		[Authorize]
		[Route("{orderId:required}/item")]
		public async Task<IActionResult> AddOrderItem([FromRoute] int orderId, [FromBody] OrderItemForCreationDTO dto)
		{
			var newOrderItem = await _orderService.AddOrderItem(orderId, dto);

			return Ok(newOrderItem);
		}


		[HttpPut]
		[Authorize]
		[Route("{orderId:required}/item/{itemId:required}")]
		public async Task<IActionResult> AddOrderItem(
			[FromRoute] int orderId,
			[FromRoute] int itemId,
			[FromBody] OrderItemForUpdateDTO dto
		)
		{
			var updatedOrderItem = await _orderService.UpdateOrderItem(orderId, itemId, dto);

			return Ok(updatedOrderItem);
		}


		[HttpDelete]
		[Authorize]
		[Route("{orderId:required}/item/{itemId:required}")]
		public async Task<IActionResult> DeleteOrderItem([FromRoute] int orderId, [FromRoute] int itemId)
		{
			var deletedOrderItem = await _orderService.DeleteOrderItem(orderId, itemId);

			return Ok(deletedOrderItem);
		}


		[HttpPut]
		[Authorize]
		[Route("{orderId:required}/delivery")]
		public async Task<IActionResult> RequestOrderDelivery([FromRoute] int orderId, [FromBody] OrderDeliveryRequest deliveryRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var requestedOrder = await _orderService.RequestDelivery(orderId, deliveryRequest);

			return Ok(requestedOrder);
		}


		[HttpPut]
		[Authorize]
		[Route("{orderId:required}/cancellation")]
		public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
		{
			var canceledOrder = await _orderService.CancelOrder(orderId);

			return Ok(canceledOrder);
		}

	}
}
