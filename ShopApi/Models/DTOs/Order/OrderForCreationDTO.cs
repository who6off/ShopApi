using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.DTOs.Order
{
	public class OrderForCreationDTO : IValidatableObject
	{
		[Required]
		public bool? IsRequestedForDelivery { get; set; }

		public string? DeliveryAddress { get; set; }

		[Required]
		public OrderItemForCreationDTO[]? OrderItems { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var errors = new List<ValidationResult>();

			if (OrderItems!.Length < 1)
			{
				errors.Add(new ValidationResult("Order must have at least one item!", new string[] { "OrderItems" }));
			}

			if (IsRequestedForDelivery.HasValue && IsRequestedForDelivery.Value && string.IsNullOrEmpty(DeliveryAddress))
			{
				errors.Add(new ValidationResult("Delivery address is mandatory for delivery!", new string[] { "DeliveryAddress" }));
			}

			return errors;
		}
	}
}
