using HelloApi.Authorization;
using HelloApi.Models;
using HelloApi.Repositories;

namespace HelloApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<User> Register(User user)
        {
            var result = await _userRepository.Add(user);
            return result;
        }

        public async Task<string?> Login(LoginRequest loginRequest)
        {
            var user = await _userRepository.FindByEmail(loginRequest.Email);

            if (user is null)
                throw new Exception("User not found!");

            if (!_passwordHasher.Verify(loginRequest.Password, user.PasswordHash))
                throw new Exception("Incorrect password!");

            var token = _tokenGenerator.Generate(user);
            return token;
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
