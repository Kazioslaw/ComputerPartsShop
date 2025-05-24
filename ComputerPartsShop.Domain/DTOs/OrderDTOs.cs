namespace ComputerPartsShop.Domain.DTOs
{
	public record OrderRequest(string? Username, string? Email, List<int> ProductIDList, decimal Total, Guid AddressID, Guid CustomerPaymentSystemID);
	public record OrderResponse(int ID, Guid CustomerID, List<int> ProductIDList, decimal Total, Guid AddressID, string Status,
		DateTime OrderedAt, DateTime? SendAt, List<int> PaymentIDList);
	public record DetailedOrderResponse(int ID, CustomerResponse Customer, List<ProductInOrderResponse> Products, decimal Total, AddressResponse Address, string Status,
		DateTime OrderedAt, DateTime? SendAt, List<PaymentInOrderResponse> Payments);
	public record ProductInOrderResponse(int ID, string Name, decimal UnitPrice, int Quantity);
	public record PaymentInOrderResponse(int ID, CustomerPaymentSystemResponse PaymentSystem, decimal Total, string Method, string Status,
		DateTime? PaymentStartAt, DateTime? PaidAt);
}
