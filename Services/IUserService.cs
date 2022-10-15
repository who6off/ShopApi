using HelloApi.Authorization;
using HelloApi.Models;

namespace HelloApi.Services
{
    public interface IUserService
    {
        public Task<User> Register(User user);

        public Task<string?> Login(LoginRequest loginRequest);

        public Task<User[]> GetAll();
        public Task<Role> AddRole(Role role);


        public Task<Role[]> GetAllRoles();
    }
}
