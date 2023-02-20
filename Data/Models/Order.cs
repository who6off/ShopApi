using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Data.Models
{
	public class Order
	{
		public int Id { get; set; }

		[Required]
		[Column(TypeName = "date")]
		public DateTime Date { get; set; }

		[Required]
		public bool IsRequestedForDelivery { get; set; } = false;

		[StringLength(200)]
		public string? DeliveryAddress { get; set; }

		public int? BuyerId { get; set; }

		[ForeignKey("BuyerId")]
		public virtual User? Buyer { get; set; }

		[Required]
		public bool IsCanceled { get; set; } = false;

		[Required]
		public bool IsDelivered { get; set; } = false;

		public DateTime? DeliveryDate { get; set; }

		public virtual ICollection<OrderItem> OrderItems { get; set; }


		public OrderItem[] GetItems() => OrderItems.ToArray();
	}
}
