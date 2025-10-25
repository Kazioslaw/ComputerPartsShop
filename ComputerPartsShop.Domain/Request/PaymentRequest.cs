using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentRequest(Guid UserPaymentSystemId, int OrderId, decimal Total, PaymentMethod Method, PaymentStatus Status, DateTime? PaymentStartAt, DateTime? PaidAt);
	public record UpdatePaymentRequest(PaymentStatus Status);
}