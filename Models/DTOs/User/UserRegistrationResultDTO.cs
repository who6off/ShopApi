namespace ShopApi.Models.DTOs.User
{
	public class UserRegistrationResultDTO
	{
		public string Token { get; set; }

		public UserDTO User { get; set; }
	}
}
