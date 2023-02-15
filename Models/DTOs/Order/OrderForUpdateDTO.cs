using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.DTOs.Order
{
	public class OrderForUpdateDTO : IValidatableObject
	{
		[Required]
		public bool? IsRequestedForDelivery { get; set; }

		[Required]
		public OrderItemForCreationDTO[]? OrderItems { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var errors = new List<ValidationResult>();

			if (OrderItems!.Length < 1)
			{
				errors.Add(new ValidationResult("Order must have at least one item", new string[] { "OrderItems" })); ;
			}

			return errors;
		}
	}
}
