using ShopApi.Models.DTOs.User;

namespace ShopApi.Models.User
{
	public class UserRegistrationResult
	{
		public string Token { get; set; }
		public UserDTO User { get; set; }
	}
}
