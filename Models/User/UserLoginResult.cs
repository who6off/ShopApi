namespace ShopApi.Models.User
{
	public class UserLoginResult
	{
		public string Token { get; set; }
		public Data.Models.User User { get; set; }
	}
}
