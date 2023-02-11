namespace ShopApi.Models.DTOs.Product
{
	public class ProductDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int? CategoryId { get; set; }
		public string? CategoryName { get; set; }
		public bool? IsForAdults { get; set; }
		public int SellerId { get; set; }
		public string? SellerName { get; set; }
		public string? Image { get; set; }
	}
}
