using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Order;

namespace ShopApi.Services.Interfaces
{
	public interface IOrderService
	{
		public Task<IPageData<OrderDTO>> Get(OrderSearchParameters searchParameters);

		public Task<OrderDTO> GetById(int id);

		public Task<OrderDTO> Add(OrderForCreationDTO dto);

		public Task<OrderDTO> Update(int id, OrderForUpdateDTO dto);

		public Task<OrderDTO> Delete(int id);


		public Task<OrderItemDTO> AddOrderItem(int orderId, OrderItemForCreationDTO dto);

		public Task<OrderItemDTO> UpdateOrderItem(int orderId, int itemId, OrderItemForUpdateDTO dto);

		public Task<OrderItemDTO> DeleteOrderItem(int orderId, int itemId);

	}
}
