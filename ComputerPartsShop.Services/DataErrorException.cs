namespace ComputerPartsShop.Services
{
	public class DataErrorException : Exception
	{
		public int StatusCode { get; }

		public DataErrorException(int statusCode, string message) : base(message)
		{
			StatusCode = statusCode;
		}
	}
}
