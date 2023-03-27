namespace ShopApi.Data.Models.SearchParameters
{
	public class ProductSearchParameters : ASearchParameters
	{
		public string? Name { get; set; }

		public int[]? Categories { get; set; }

		public int[]? Sellers { get; set; }

		public bool? IsForAdults { get; set; }

		public decimal? MinPrice { get; set; }

		public decimal? MaxPrice { get; set; }

		public ProductSearchParameters() : base() { }
	}
}
