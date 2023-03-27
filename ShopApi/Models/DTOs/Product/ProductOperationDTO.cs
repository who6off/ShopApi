using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.DTOs.Product
{
	public class ProductOperationDTO
	{
		[Required]
		public string? Name { get; set; }

		public int? SellerId { get; set; }
	}
}
