using System.Net;

namespace InvoiceApp.Helpers.Exceptions
{
	public class AppException : Exception
	{
		public virtual int ErrorCode { get; protected set; } = (int)HttpStatusCode.InternalServerError;

		public virtual string ErrorDetails { get; protected set; } = "Internal server error!";


		public AppException() : base(String.Empty) { }


		public AppException(string message) : base(message) { }
	}
}
