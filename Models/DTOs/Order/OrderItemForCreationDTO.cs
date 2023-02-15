﻿using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.DTOs.Order
{
	public class OrderItemForCreationDTO
	{
		[Required]
		public int? ProductId { get; set; }


		[Required]
		[Range(1, int.MaxValue)]
		public int? Amount { get; set; }
	}
}
