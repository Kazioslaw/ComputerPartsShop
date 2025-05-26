namespace ComputerPartsShop.Domain.DTO
{
	public record OrderResponse(int Id, Guid CustomerId, List<int> ProductIdList, decimal Total, Guid AddressId, string Status,
		DateTime? OrderedAt, DateTime? SendAt, List<int> PaymentIdList);
	public record DetailedOrderResponse(int Id, CustomerResponse Customer, List<ProductInOrderResponse> Products, decimal Total, AddressResponse Address, string Status,
		DateTime OrderedAt, DateTime? SendAt, List<PaymentInOrderResponse> Payments);
	public record ProductInOrderResponse(int Id, string Name, decimal UnitPrice, int Quantity);
	public record PaymentInOrderResponse(int Id, CustomerPaymentSystemResponse PaymentSystem, decimal Total, string Method, string Status,
		DateTime? PaymentStartAt, DateTime? PaidAt);
}