using System.Net;

namespace ComputerPartsShop.Services
{
	public class DataErrorException : Exception
	{
		public HttpStatusCode StatusCode { get; }

		public DataErrorException(HttpStatusCode statusCode, string message) : base(message)
		{
			StatusCode = statusCode;
		}
	}
}
