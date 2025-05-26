namespace ComputerPartsShop.Domain.DTO
{
	public record CustomerPaymentSystemResponse(Guid Id, string? Username, string? Email, string ProviderName, string PaymentReference);
	public record DetailedCustomerPaymentSystemResponse(Guid Id, string? Username, string? Email, string ProviderName, string PaymentReference,
		List<PaymentInCustomerPaymentSystemResponse> Payments);
	public record PaymentInCustomerPaymentSystemResponse(int Id, int OrderId, decimal Total, string Method, string Status, DateTime PaymentStartAt, DateTime? PaidAt);
}