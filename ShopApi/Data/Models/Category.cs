using System.ComponentModel.DataAnnotations;

namespace ShopApi.Data.Models
{
	public class Category
	{
		public int Id { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 1)]
		public string Name { get; set; }

		[Required]
		public int DisplayOrder { get; set; }


		[Required]
		public bool IsForAdults { get; set; }


		public virtual ICollection<Product> Products { get; set; }
	}
}
