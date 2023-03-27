namespace ShopApi.Models.DTOs.User
{
	public class UserDTO
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string SecondName { get; set; }
		public int RoleId { get; set; }
		public string RoleName { get; set; }
		public DateTime BirthDate { get; set; }
	}
}
