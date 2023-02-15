using ShopApi.Models.DTOs.Product;

namespace ShopApi.Models.DTOs.Order
{
	public class OrderItemDTO
	{
		public ProductDTO Product { get; set; }

		public uint Amount { get; set; }
	}
}
