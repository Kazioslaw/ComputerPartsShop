using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.DTO
{
	public record CustomerPaymentSystemResponse(Guid Id, string? Username, string? Email, string ProviderName, string PaymentReference);
	public record DetailedCustomerPaymentSystemResponse(Guid Id, string? Username, string? Email, string ProviderName, string PaymentReference,
		List<PaymentInCustomerPaymentSystemResponse> Payments);
	public record PaymentInCustomerPaymentSystemResponse(int Id, int OrderId, decimal Total, PaymentMethod Method, PaymentStatus Status, DateTime PaymentStartAt, DateTime? PaidAt);
}