using ShopApi.Authentication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.DTOs.User
{
	public class UserForRegistrationDTO : IValidatableObject
	{
		[Required]
		[StringLength(320, MinimumLength = 3)]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		[StringLength(40, MinimumLength = 1)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 1)]
		public string SecondName { get; set; }

		[Required]
		public bool? IsSeller { get; set; }

		[Required]
		public DateTime? BirthDate { get; set; }


		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var errors = new List<ValidationResult>();
			var validator = validationContext.GetService<IValidator>();

			if (!validator.ValidateEmail(Email))
			{
				errors.Add(new ValidationResult("Incorrect email address", new string[] { "Email" }));
			}

			if (!validator.ValidatePassword(Password))
			{
				errors.Add(new ValidationResult("Inappropriate password", new string[] { "Password" }));
			}

			return errors;
		}
	}
}
