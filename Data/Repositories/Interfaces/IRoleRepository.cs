using ShopApi.Data.Models;

namespace ShopApi.Data.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Role> Add(Role role);
        public Task<Role[]> GetAll();

    }
}
