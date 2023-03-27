using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Data.Models
{
	public class Product
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 1)]
		public string Name { get; set; }

		[Required]
		[Column(TypeName = "decimal(8,2)")]
		public decimal Price { get; set; }

		public int? CategoryId { get; set; }

		[ForeignKey("CategoryId")]
		public virtual Category? Category { get; set; }

		[Required]
		public int SellerId { get; set; }

		[Required]
		[ForeignKey("SellerId")]
		public virtual User Seller { get; set; }

		public string? Image { get; set; }

		public virtual ICollection<OrderItem> OrderItems { get; set; }


		public string? GetCategoryName() => Category?.Name;

		public bool? GetIsForAdults() => Category?.IsForAdults;

		public string? GetSellerName() => (Seller is null) ? null : $"{Seller.FirstName} {Seller.SecondName}";
	}
}
