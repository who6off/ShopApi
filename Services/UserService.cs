using HelloApi.Models;
using HelloApi.Repositories;

namespace HelloApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Register(User user)
        {
            var result = await _userRepository.Add(user);
            return result;
        }

        public async Task<User[]> GetAll()
        {
            var result = await _userRepository.GetAll();
            return result;
        }

        public async Task<Role> AddRole(Role role)
        {
            return await _userRepository.AddRole(role);
        }

        public async Task<Role[]> GetAllRoles()
        {
            return await _userRepository.GetAllRoles();
        }
    }
}
