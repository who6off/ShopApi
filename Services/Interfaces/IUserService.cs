using ShopApi.Authentication;
using ShopApi.Models;
using ShopApi.Models.Requests;
using ShopApi.Models.Responses;

namespace ShopApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<RegistrationResponce> Register(RegistrationRequest request);

        public Task<string?> Login(LoginRequest loginRequest);


        public Task<User> GetById(int id);
        public Task<User[]> GetAll();

        public Task<Role> AddRole(Role role);


        public Task<Role[]> GetAllRoles();
    }
}
