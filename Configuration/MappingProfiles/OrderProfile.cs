using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.Models.DTOs.Order;

namespace ShopApi.Configuration.MappingProfiles
{
	public class OrderProfile : Profile
	{
		public OrderProfile()
		{
			CreateMap<Order, OrderDTO>();
			CreateMap<OrderItem, OrderItemDTO>();

			CreateMap<OrderForCreationDTO, Order>();
			CreateMap<OrderItemForCreationDTO, OrderItem>();

			CreateMap<OrderForUpdateDTO, Order>();
			CreateMap<OrderItemForUpdateDTO, OrderItem>();

		}
	}
}
