namespace ShopApi.Models.DTOs.User
{
	public class UserLoginResultDTO
	{
		public string Token { get; set; }

		public UserDTO User { get; set; }
	}
}
