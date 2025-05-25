namespace ComputerPartsShop.Domain.DTOs
{
	public record PaymentRequest(Guid CustomerPaymentSystemID, int OrderID, decimal Total, string Method);
	public record PaymentResponse(int ID, Guid CustomerPaymentSystemID, int OrderID, decimal Total, string Method, string Status, DateTime PaymentStartAt, DateTime? PaidAt);
	public record DetailedPaymentResponse(int ID, CustomerPaymentSystemResponse CustomerPaymentSystemResponse, OrderInPaymentResponse Order, decimal Total,
		string Method, string Status, DateTime PaymentStartAt, DateTime? PaidAt);
	public record OrderInPaymentResponse(int ID, string Username, string Email, List<ProductInPaymentResponse> Product, AddressResponse Address, string Status,
		DateTime OrderedAt, DateTime? SendAt);

	public record ProductInPaymentResponse(string Name, int Quantity);
}
