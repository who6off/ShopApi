using ShopApi.Authentication;
using ShopApi.Authentication.Interfaces;
using ShopApi.Data.Models;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Extensions;
using ShopApi.Models.User;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
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


		public async Task<UserRegistrationResult?> Register(User user, string password)
		{
			user.PasswordHash = _passwordHasher.Hash(password);

			var newUser = await _userRepository.Add(user);

			if (newUser is null)
			{
				return null;
			}

			var token = _tokenGenerator.Generate(user);
			return new UserRegistrationResult() { User = newUser, Token = token };
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
