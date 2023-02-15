using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Data.Models
{
	public class OrderItem
	{
		public int Id { get; set; }

		public int? OrderId { get; set; }

		[ForeignKey("OrderId")]
		public virtual Order? Order { get; set; }

		public int? ProductId { get; set; }

		[ForeignKey("ProductId")]
		public virtual Product? Product { get; set; }

		[Required]
		public uint Amount { get; set; }


		public OrderItem Copy()
		{
			return new OrderItem()
			{
				Id = Id,
				OrderId = OrderId,
				ProductId = ProductId,
				Amount = Amount,
			};
		}
	}
}
