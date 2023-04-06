using ShopApi.Authentication;

namespace ShopApi.Test.Authentication
{
	public class ValidatorTest
	{
		private readonly Validator _validator;

		public ValidatorTest()
		{
			_validator = new Validator();
		}


		[Theory]
		[InlineData("ivan.huzikov@gmail.com")]
		[InlineData("joker777@yahoo.com")]
		[InlineData("batman@test.ca")]
		public void ValidateEmail_EmaiIsValid_ReturnsTrue(string email)
		{
			Assert.True(_validator.ValidateEmail(email));
		}


		[Theory]
		[InlineData("")]
		[InlineData("johndoe@test")]
		[InlineData("hello@q.w")]
		public void ValidateEmail_EmailIsInvalid_ReturnsFalse(string email)
		{
			Assert.False(_validator.ValidateEmail(email));
		}


		[Theory]
		[InlineData("123-Qwe")]
		[InlineData("passworD-2023")]
		[InlineData("12gd64s!e0nQ")]
		public void ValidatePassword_PasswordIsValid_ReturnsTrue(string password)
		{
			Assert.True(_validator.ValidatePassword(password));
		}


		[Theory]
		[InlineData("123qwe")]
		[InlineData("password")]
		[InlineData("10041998")]
		[InlineData("strong_password_2023")]
		public void ValidatePassword_PasswordIsInvalid_ReturnsFalse(string password)
		{
			Assert.False(_validator.ValidatePassword(password));
		}
	}
}
