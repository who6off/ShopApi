namespace ShopApi.Models.DTOs.Order
{
	public class OrderDTO
	{
		public int Id { get; set; }

		public int BuyerId { get; set; }

		public DateTime Date { get; set; }

		public OrderItemDTO[] Items { get; set; }

		public bool IsRequestedForDelivery { get; set; }

		public bool IsDelivered { get; set; }

		public bool IsCanceled { get; set; }
		public DateTime? DeliveryDate { get; set; }
	}
}
