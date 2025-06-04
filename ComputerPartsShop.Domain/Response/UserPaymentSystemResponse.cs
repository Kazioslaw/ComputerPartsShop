using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.DTO
{
	public record UserPaymentSystemResponse(Guid Id, string? Username, string? Email, string ProviderName, string PaymentReference);
	public record DetailedUserPaymentSystemResponse(Guid Id, string? Username, string? Email, string ProviderName, string PaymentReference,
		List<PaymentInUserPaymentSystemResponse> Payments);
	public record PaymentInUserPaymentSystemResponse(Guid Id, int OrderId, decimal Total, PaymentMethod Method, PaymentStatus Status, DateTime PaymentStartAt, DateTime? PaidAt);
}