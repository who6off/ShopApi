﻿using ShopApi.Authentication.Interfaces;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Extensions;
using ShopApi.Helpers.Interfaces;
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


		public async Task<UserLoginResult?> Login(string email, string password)
		{
			var user = await _userRepository.FindByEmail(email);

			if (
				(user is null) ||
				(!_passwordHasher.Verify(password, user.PasswordHash))
			)
			{
				return null;
			}

			if (!_passwordHasher.Verify(password, user.PasswordHash))
			{
				throw new Exception("Incorrect password!");
			}

			var token = _tokenGenerator.Generate(user);

			return new UserLoginResult() { User = user, Token = token };
		}


		public async Task<User> GetById(int id)
		{
			var result = await _userRepository.GetById(id);
			return result;
		}


		public async Task<IPageData<User>> Get(UserSearchParameters searchParameters)
		{
			var result = await _userRepository.Get(searchParameters);
			return result;
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
