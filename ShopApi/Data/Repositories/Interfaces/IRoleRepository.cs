using ShopApi.Data.Models;

namespace ShopApi.Data.Repositories.Interfaces
{
	public interface IRoleRepository
	{
		public Task<Role[]> GetAll();

		public Task<Role?> GetById(int id);

		public Task<Role?> GetByName(string name);

	}
}
