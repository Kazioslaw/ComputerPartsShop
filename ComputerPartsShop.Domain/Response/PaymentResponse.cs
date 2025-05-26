namespace ComputerPartsShop.Domain.DTO
{
	public record PaymentResponse(int Id, Guid CustomerPaymentSystemId, int OrderId, decimal Total, string Method, string Status, DateTime? PaymentStartAt, DateTime? PaidAt);
	public record DetailedPaymentResponse(int Id, CustomerPaymentSystemResponse CustomerPaymentSystemResponse, OrderInPaymentResponse Order, decimal Total,
		string Method, string Status, DateTime PaymentStartAt, DateTime? PaidAt);
	public record OrderInPaymentResponse(int Id, string Username, string Email, List<ProductInPaymentResponse> Product, AddressResponse Address, string Status,
		DateTime OrderedAt, DateTime? SendAt);

	public record ProductInPaymentResponse(string Name, int Quantity);
}