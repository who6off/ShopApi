namespace ShopApi.Data.Models.SearchParameters
{
	public class UserSearchParameters : ASearchParameters
	{
		public string? FirstName { get; set; }

		public string? SecondName { get; set; }

		public string? Email { get; set; }

		public int? RoleId { get; set; }

		public DateTime? BirthDate { get; set; }

		public UserSearchParameters() : base() { }
	}
}
