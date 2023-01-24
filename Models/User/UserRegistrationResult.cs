namespace ShopApi.Models.User
{
	public class UserRegistrationResult
	{
		public string Token { get; set; }
		public Data.Models.User User { get; set; }
	}
}
