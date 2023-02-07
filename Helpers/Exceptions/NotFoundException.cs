using System.Net;

namespace InvoiceApp.Helpers.Exceptions
{
	public class NotFoundException : AppException
	{
		public override int ErrorCode { get; protected set; } = (int)HttpStatusCode.NotFound;

		public override string ErrorDetails { get; protected set; } = "Not found!";

		public NotFoundException() : base() { }

		public NotFoundException(string message) : base(message) { }
	}
}
