using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.Models.DTOs.Role;

namespace ShopApi.Configuration.MappingProfiles
{
	public class RoleProfile : Profile
	{
		public RoleProfile()
		{
			CreateMap<Role, RoleDTO>();
		}
	}
}
