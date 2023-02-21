using AutoMapper;
using ShopApi.Authentication;
using ShopApi.Authentication.Interfaces;
using ShopApi.Authorization;
using ShopApi.Configuration;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Extensions;
using ShopApi.Helpers.Exceptions;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Role;
using ShopApi.Models.DTOs.User;
using ShopApi.Models.Requests.User;
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
		private readonly IConfiguration _configuration;
		private readonly HttpContext _httpContext;

		public UserService(
			IUserRepository userRepository,
			IRoleRepository roleRepository,
			IMapper mapper,
			IPasswordHasher passwordHasher,
			ITokenGenerator tokenGenerator,
			IValidator validator,
			IConfiguration configuration,
			IHttpContextAccessor httpContextAccessor
		)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_mapper = mapper;
			_passwordHasher = passwordHasher;
			_tokenGenerator = tokenGenerator;
			_validator = validator;
			_configuration = configuration;

			if (httpContextAccessor.HttpContext is null)
			{
				throw new AppException("User service error!");
			}

			_httpContext = httpContextAccessor.HttpContext;
		}


		public async Task<UserRegistrationResult?> Register(UserForRegistrationDTO dto)
		{
			var user = _mapper.Map<User>(dto);

			if (dto.IsSeller is null)
			{
				throw new AppException("Not enough data for registration");
			}

			if (dto.IsSeller.Value && !IsAgeAppropriateForTheRole(UserRoles.Seller, dto.BirthDate.Value))
			{
				throw new AppException("Seller must be adult age");
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
			var user = await _userRepository.GetByEmail(request.Email);

			if (user is null)
			{
				throw new NotFoundException("User not found!");
			}

			if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
			{
				throw new AccessDeniedException("Incorrect password!");
			}

			var userLoginResult = new UserLoginResult()
			{
				User = _mapper.Map<UserDTO>(user),
				Token = _tokenGenerator.Generate(user)
			};

			return userLoginResult;
		}


		public async Task<UserProfileUpdateResult> UpdateProfile(UserProfileUpdateDTO dto)
		{
			var userId = _httpContext.User.GetUserId();
			var user = await _userRepository.GetById(userId.Value);

			if (user is null)
			{
				throw new NotFoundException("User is not found!");
			}

			if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
			{
				throw new AccessDeniedException("Incorrect password!");
			}

			_mapper.Map(dto, user);

			if (!string.IsNullOrEmpty(dto.NewPassword))
			{
				user.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
			}

			var updatedUser = await _userRepository.Update(user);

			if (updatedUser is null)
			{
				throw new AppException("Profile update error!");
			}

			var profileUpdateResult = new UserProfileUpdateResult()
			{
				User = _mapper.Map<UserDTO>(updatedUser),
				Token = _tokenGenerator.Generate(user)
			};

			return profileUpdateResult;
		}


		public async Task<UserDTO?> GetById(int id)
		{
			var user = await _userRepository.GetById(id);

			if (user is null)
			{
				throw new NotFoundException("User not found!");
			}

			var userDTO = _mapper.Map<UserDTO>(user);
			return userDTO;
		}


		public async Task<IPageData<UserDTO>> Get(UserSearchParameters searchParameters)
		{
			var result = await _userRepository.Get(searchParameters);
			var mapResult = result.Map<UserDTO>(_mapper);
			return mapResult;
		}


		public async Task<UserDTO?> Add(UserForCreationDTO dto)
		{
			var user = _mapper.Map<User>(dto);

			var role = await _roleRepository.GetById(dto.RoleId.Value);

			if (role is null)
			{
				throw new NotFoundException("Role not found!");
			}

			if (!IsAgeAppropriateForTheRole(role.Name, dto.BirthDate.Value))
			{
				throw new ClientInputException("Inappropriate user age!");
			}

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


		public async Task<RoleDTO[]> GetRoles()
		{
			var data = await _roleRepository.GetAll();
			var dataMap = _mapper.Map<RoleDTO[]>(data);
			return dataMap;
		}


		private bool IsAgeAppropriateForTheRole(string roleName, DateTime birthDate)
		{
			if (
				(DateTime.Now.GetYearDifference(birthDate) < _configuration.GetAdultAge()) &&
				((roleName == UserRoles.Seller) || (roleName == UserRoles.Admin)))
			{
				return false;
			}

			return true;
		}
	}
}
