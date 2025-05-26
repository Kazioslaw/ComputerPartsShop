namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentProviderResponse(int Id, string Name, List<Guid> CustomerPaymentSystemIdList);
	public record DetailedPaymentProviderResponse(int Id, string Name, List<CustomerPaymentSystemResponse> CustomerPaymentSystemList);
}