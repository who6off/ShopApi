namespace ShopApi.Data.Models.SearchParameters
{
	public class OrderSearchParameters : ASearchParameters
	{
		public int? BuyerId { get; set; }

		public int? SellerId { get; set; }

		public DateTime? Date { get; set; }

		public DateTime? DateFrom { get; set; }

		public DateTime? DateTo { get; set; }

		public bool? IsRequestedForDelivery { get; set; }

		public bool? IsCanceled { get; set; }

		public bool? IsDelivered { get; set; }

		public DateTime? DeliveryDate { get; set; }
		public OrderSearchParameters() : base() { }
	}
}
