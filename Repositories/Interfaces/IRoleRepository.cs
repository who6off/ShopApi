using ShopApi.Models;

namespace ShopApi.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Role> Add(Role role);
        public Task<Role[]> GetAll();

    }
}
