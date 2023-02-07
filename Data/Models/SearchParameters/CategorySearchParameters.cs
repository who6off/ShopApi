namespace ShopApi.Data.Models.SearchParameters
{
	public class CategorySearchParameters : ASearchParameters
	{
		public bool? IsForAdults { get; set; }

		public CategorySearchParameters() : base() { }
	}
}
