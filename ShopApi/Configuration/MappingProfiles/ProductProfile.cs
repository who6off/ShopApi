using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.Models.DTOs.Product;

namespace ShopApi.Configuration.MappingProfiles
{
	public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			CreateMap<Product, ProductDTO>();

			CreateMap<ProductForCreationDTO, Product>();

			CreateMap<ProductForUpdateDTO, Product>();
		}
	}
}
