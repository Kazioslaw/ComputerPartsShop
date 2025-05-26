namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentProviderResponse(int ID, string Name, List<Guid> CustomerPaymentSystemIDList);
	public record DetailedPaymentProviderResponse(int ID, string Name, List<CustomerPaymentSystemResponse> CustomerPaymentSystemList);
}