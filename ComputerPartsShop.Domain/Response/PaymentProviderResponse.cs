namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentProviderResponse(int Id, string Name, List<Guid> UserPaymentSystemIdList);
	public record DetailedPaymentProviderResponse(int Id, string Name, List<UserPaymentSystemResponse> UserPaymentSystemList);
}