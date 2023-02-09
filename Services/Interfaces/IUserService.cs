using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Role;
using ShopApi.Models.DTOs.User;
using ShopApi.Models.Requests;
using ShopApi.Models.User;

namespace ShopApi.Services.Interfaces
{
	public interface IUserService
	{
		public Task<UserRegistrationResult> Register(UserForRegistrationDTO dto);

		public Task<UserLoginResult?> Login(LoginRequest request);

		public Task<UserDTO?> GetById(int id);

		public Task<IPageData<UserDTO>> Get(UserSearchParameters searchParameters);

		public Task<UserDTO?> Add(UserForCreationDTO dto);

		public Task<UserDTO?> Update(int id, UserForUpdateDTO dto);

		public Task<UserDTO?> Delete(int id);

		public Task<RoleDTO> AddRole(Role role);

		public Task<IPageData<RoleDTO>> GetRoles(RoleSearchParameters searchParameters);
	}
}
