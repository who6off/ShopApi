using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.Models.DTOs.Category;

namespace ShopApi.Configuration.MappingProfiles
{
	public class CategoryProfile : Profile
	{
		public CategoryProfile()
		{
			CreateMap<Category, CategoryDTO>().ReverseMap();
			CreateMap<CategoryForCreationDTO, Category>();
			CreateMap<CategoryForUpdateDTO, Category>();
		}
	}
}
