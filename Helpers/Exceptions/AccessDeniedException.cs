using System.Net;

namespace InvoiceApp.Helpers.Exceptions
{
	public class AccessDeniedException : AppException
	{
		public override int ErrorCode { get; protected set; } = (int)HttpStatusCode.Forbidden;

		public override string ErrorDetails { get; protected set; } = "Access denied!";

		public AccessDeniedException() : base() { }

		public AccessDeniedException(string message) : base(message) { }
	}
}
