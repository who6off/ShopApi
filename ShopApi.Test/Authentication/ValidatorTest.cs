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
		public void VaidateEmail_EmaiIsValid_ReturnsTrue(string email)
		{
			Assert.True(_validator.ValidateEmail(email));
		}
	}
}
