using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.Models.DTOs.User;

namespace ShopApi.Configuration.MappingProfiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<User, UserDTO>();

			CreateMap<UserForRegistrationDTO, User>();

			CreateMap<UserForCreationDTO, User>();

			CreateMap<UserForUpdateDTO, User>();
		}
	}
}
