namespace ShopApi.Data.Models.SearchParameters
{
	public class CategorySearchParameters : ASearchParameters
	{
		public string? Name { get; set; }
		public bool? IsForAdults { get; set; }

		public CategorySearchParameters() : base() { }
	}
}
