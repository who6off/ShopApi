using ShopApi.Authentication;
using ShopApi.Data.Models;
using ShopApi.Models.User;

namespace ShopApi.Services.Interfaces
{
	public interface IUserService
	{
		public Task<UserRegistrationResult> Register(User user, string password);

		public Task<string?> Login(LoginRequest loginRequest);


		public Task<User> GetById(int id);
		public Task<User[]> GetAll();

		public Task<Role> AddRole(Role role);


		public Task<Role[]> GetAllRoles();
	}
}
