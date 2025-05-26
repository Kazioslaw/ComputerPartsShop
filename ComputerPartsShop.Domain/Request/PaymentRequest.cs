namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentRequest(Guid CustomerPaymentSystemId, int OrderId, decimal Total, string Method, string? Status, DateTime? PaymentStartAt, DateTime? PaidAt);
}