namespace ShopApi.Data.Models.SearchParameters
{
	public class OrderSearchParameters : ASearchParameters
	{
		public int? BuyerId { get; set; }

		public OrderSearchParameters() : base() { }
	}
}
