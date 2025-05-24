namespace ComputerPartsShop.Domain.DTOs
{
	public record CustomerPaymentSystemRequest(string? Username, string? Email, string ProviderName, string PaymentReference);
	public record CustomerPaymentSystemResponse(Guid ID, string? Username, string? Email, string ProviderName, string PaymentReference);
	public record DetailedCustomerPaymentSystemResponse(Guid ID, string? Username, string? Email, string ProviderName, string PaymentReference,
		List<PaymentInCustomerPaymentSystemResponse> Payments);
	public record PaymentInCustomerPaymentSystemResponse(int ID, int OrderID, decimal Total, string Method, string Status, DateTime PaymentStartAt, DateTime? PaidAt);
}
