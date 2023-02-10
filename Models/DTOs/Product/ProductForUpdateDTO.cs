using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.DTOs.Product
{
	public class ProductForUpdateDTO
	{
		[Required]
		public string? Name { get; set; }

		[Required]
		public decimal? Price { get; set; }

		[Required]
		public int? CategoryId { get; set; }

		public int? SellerId { get; set; }

		public IFormFile? Image { get; set; }
	}
}
