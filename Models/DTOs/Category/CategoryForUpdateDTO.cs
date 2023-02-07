using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.DTOs.Category
{
	public class CategoryForUpdateDTO
	{
		[Required]
		[StringLength(50, MinimumLength = 1)]
		public string? Name { get; set; }

		[Required]
		public bool? IsForAdults { get; set; }
	}
}
