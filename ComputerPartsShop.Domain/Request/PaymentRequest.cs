namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentRequest(Guid CustomerPaymentSystemID, int OrderID, decimal Total, string Method);
}