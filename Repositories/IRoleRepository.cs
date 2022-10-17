using HelloApi.Models;

namespace HelloApi.Repositories
{
    public interface IRoleRepository
    {
        public Task<Role> Add(Role role);
        public Task<Role[]> GetAll();

    }
}
