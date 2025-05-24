namespace ComputerPartsShop.Domain.DTOs
{
	public record PaymentProviderRequest(string Name);
	public record PaymentProviderResponse(int ID, string Name, List<Guid> CustomerPaymentSystemID);
	public record DetailedPaymentProviderResponse(int ID, string Name, List<CustomerPaymentSystemResponse> CustomerPaymentSystemList);
}
