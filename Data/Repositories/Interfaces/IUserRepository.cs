using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Data.Repositories.Interfaces
{
	public interface IUserRepository
	{
		public Task<User?> GetById(int id);

		public Task<User?> GetByEmail(string email);

		public Task<IPageData<User>> Get(UserSearchParameters searchParameters);

		public Task<User?> Add(User user);

		public Task<User?> Update(User user);

		public Task<User?> Delete(int id);


		public Task<string?> GetUserRoleNameById(int id);
	}
}
