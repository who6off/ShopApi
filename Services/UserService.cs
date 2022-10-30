using HelloApi.Authentication;
using HelloApi.Extensions;
using HelloApi.Models;
using HelloApi.Models.Requests;
using HelloApi.Models.Responses;
using HelloApi.Repositories.Interfaces;
using HelloApi.Services.Interfaces;

namespace HelloApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IValidator _validator;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IValidator validator)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _validator = validator;
        }

        public async Task<RegistrationResponce> Register(RegistrationRequest request)
        {
            if (!_validator.ValidateEmail(request.Email))
                throw new Exception("Incorrect email address");

            if (!_validator.ValidatePassword(request.Password))
                throw new Exception("Incorrect password");

            var user = new User()
            {
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                RoleId = request.RoleId,
                FirstName = request.FirstName.FirstCharToUpper(),
                SecondName = request.SecondName.FirstCharToUpper(),
                BirthDate = DateTime.Parse(request.BirthDate)
            };

            var newUser = await _userRepository.Add(user);
            var token = _tokenGenerator.Generate(user);
            return new RegistrationResponce() { User = newUser, Token = token };
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


        public async Task<User> GetById(int id)
        {
            var result = await _userRepository.GetById(id);
            return result;
        }


        public async Task<User[]> GetAll()
        {
            var result = await _userRepository.GetAll();
            return result;
        }


        public async Task<Role> AddRole(Role role)
        {
            role.Name = role.Name.FirstCharToUpper();
            return await _roleRepository.Add(role);
        }

        public async Task<Role[]> GetAllRoles()
        {
            return await _roleRepository.GetAll();
        }
    }
}
