using AutoMapper;
using ShopApi.Authentication;
using ShopApi.Authentication.Interfaces;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Extensions;
using ShopApi.Helpers.Exceptions;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.User;
using ShopApi.Models.Requests;
using ShopApi.Models.User;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly IMapper _mapper;
		private readonly IPasswordHasher _passwordHasher;
		private readonly ITokenGenerator _tokenGenerator;
		private readonly IValidator _validator;


		public UserService(
			IUserRepository userRepository,
			IRoleRepository roleRepository,
			IMapper mapper,
			IPasswordHasher passwordHasher,
			ITokenGenerator tokenGenerator,
			IValidator validator)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_mapper = mapper;
			_passwordHasher = passwordHasher;
			_tokenGenerator = tokenGenerator;
			_validator = validator;
		}


		public async Task<UserRegistrationResult?> Register(UserForRegistrationDTO dto)
		{
			var user = _mapper.Map<User>(dto);

			if (dto.IsSeller is null)
			{
				throw new AppException("Not enough data for registration");
			}

			user.Role = (bool)dto.IsSeller
				? (await _roleRepository.GetByName(UserRoles.Seller))
				: (await _roleRepository.GetByName(UserRoles.Buyer));
			user.PasswordHash = _passwordHasher.Hash(dto.Password);

			var newUser = await _userRepository.Add(user);

			if (newUser is null)
			{
				throw new AppException("Registration error!");
			}

			var userRegistrationResult = new UserRegistrationResult()
			{
				User = _mapper.Map<UserDTO>(newUser),
				Token = _tokenGenerator.Generate(user)
			};

			return userRegistrationResult;
		}


		public async Task<UserLoginResult?> Login(LoginRequest request)
		{
			var user = await _userRepository.FindByEmail(request.Email);

			if (user is null)
			{
				throw new AppException("User not found!");
			}

			if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
			{
				throw new AppException("Incorrect password!");
			}

			var userLoginResult = new UserLoginResult()
			{
				User = _mapper.Map<UserDTO>(user),
				Token = _tokenGenerator.Generate(user)
			};

			return userLoginResult;
		}


		public async Task<UserDTO?> GetById(int id)
		{
			var user = await _userRepository.GetById(id);

			if (user is null)
			{
				return null;
			}

			var userDTO = _mapper.Map<UserDTO>(user);
			return userDTO;
		}


		public async Task<IPageData<User>> Get(UserSearchParameters searchParameters)
		{
			var result = await _userRepository.Get(searchParameters);
			return result;
		}


		public async Task<UserDTO?> Add(UserForCreationDTO dto)
		{
			var user = _mapper.Map<User>(dto);

			user.PasswordHash = _passwordHasher.Hash(dto.Password);

			var newUser = await _userRepository.Add(user);

			if (newUser is null)
			{
				throw new AppException("Creation  error!");
			}

			var userDTO = _mapper.Map<UserDTO>(newUser);
			return userDTO;
		}


		public async Task<UserDTO?> Update(int id, UserForUpdateDTO dto)
		{
			var user = await _userRepository.GetById(id);

			if (user is null)
			{
				throw new NotFoundException("User is not found");
			}

			_mapper.Map(dto, user);
			user.Role = null;
			if (dto.Password is not null)
			{
				user.PasswordHash = _passwordHasher.Hash(dto.Password);
			}

			var updatedUser = await _userRepository.Update(user);

			if (updatedUser is null)
			{
				throw new AppException("Update error!");
			}

			var userDTO = _mapper.Map<UserDTO>(updatedUser);
			return userDTO;
		}


		public async Task<UserDTO?> Delete(int id)
		{
			var user = await _userRepository.GetById(id);

			if (user is null)
			{
				throw new NotFoundException("User is not found");
			}

			var deletedUser = await _userRepository.Delete(id);

			if (deletedUser is null)
			{
				throw new AppException("Delete error!");
			}

			var userDTO = _mapper.Map<UserDTO>(deletedUser);
			return userDTO;
		}


		public async Task<Role> AddRole(Role role)
		{
			role.Name = role.Name.FirstCharToUpper();
			return await _roleRepository.Add(role);
		}


		public async Task<IPageData<Role>> GetRoles(RoleSearchParameters searchParameters)
		{
			return await _roleRepository.Get(searchParameters);
		}
	}
}
