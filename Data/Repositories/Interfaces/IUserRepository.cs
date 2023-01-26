using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;

namespace ShopApi.Data.Repositories.Interfaces
{
	public interface IUserRepository
	{
		public Task<User?> GetById(int id);
		public Task<User?> FindByEmail(string email);
		public Task<User[]> Get(UserSearchParameters searchParameters);
		public Task<User> Add(User user);
	}
}
