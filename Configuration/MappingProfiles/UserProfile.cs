using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.Models.DTOs.User;
using ShopApi.Models.User;

namespace ShopApi.Configuration.MappingProfiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<UserForCreationDTO, User>();

			CreateMap<User, UserDTO>();

			CreateMap<UserRegistrationResult, UserRegistrationResultDTO>();

			CreateMap<UserLoginResult, UserLoginResultDTO>();
		}
	}
}
