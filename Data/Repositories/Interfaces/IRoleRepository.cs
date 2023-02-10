using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Data.Repositories.Interfaces
{
	public interface IRoleRepository
	{
		public Task<Role?> Add(Role role);

		public Task<IPageData<Role>> Get(RoleSearchParameters parameters);

		public Task<Role?> GetByName(string name);

	}
}
