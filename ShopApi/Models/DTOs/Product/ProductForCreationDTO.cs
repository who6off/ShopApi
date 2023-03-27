using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.DTOs.Product
{
	public class ProductForCreationDTO : ProductOperationDTO
	{
		[Required]
		public decimal? Price { get; set; }

		[Required]
		public int? CategoryId { get; set; }


		public IFormFile? Image { get; set; }
	}
}
