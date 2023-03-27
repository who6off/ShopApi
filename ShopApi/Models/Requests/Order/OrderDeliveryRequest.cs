using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.Requests.Order
{
	public class OrderDeliveryRequest
	{
		[Required]
		public string DeliveryAddress { get; set; }
	}
}
