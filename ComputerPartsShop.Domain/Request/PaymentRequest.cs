using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentRequest(Guid CustomerPaymentSystemId, int OrderId, decimal Total, PaymentMethod Method, PaymentStatus Status, DateTime? PaymentStartAt, DateTime? PaidAt);
}