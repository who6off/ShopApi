using HelloApi.Authentication;
using HelloApi.Models;

namespace HelloApi.Services
{
    public interface IUserService
    {
        public Task<RegistrationResponce> Register(RegistrationRequest request);

        public Task<string?> Login(LoginRequest loginRequest);

        public Task<User[]> GetAll();
        public Task<Role> AddRole(Role role);


        public Task<Role[]> GetAllRoles();
    }
}
