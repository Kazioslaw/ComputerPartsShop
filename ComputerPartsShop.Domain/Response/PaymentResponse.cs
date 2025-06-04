using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentResponse(Guid Id, Guid UserPaymentSystemId, int OrderId, decimal Total, PaymentMethod Method, PaymentStatus Status, DateTime? PaymentStartAt, DateTime? PaidAt);
}