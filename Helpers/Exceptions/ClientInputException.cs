using System.Net;

namespace ShopApi.Helpers.Exceptions
{
	public class ClientInputException : AppException
	{
		public override int ErrorCode { get; protected set; } = (int)HttpStatusCode.BadRequest;

		public override string ErrorDetails { get; protected set; } = "Incorrect data input!";

		public ClientInputException() : base() { }

		public ClientInputException(string message) : base(message) { }

	}
}
