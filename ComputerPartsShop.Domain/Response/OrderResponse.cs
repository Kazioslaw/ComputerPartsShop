using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.DTO
{
	public record OrderResponse(int Id, Guid CustomerId, List<ProductInOrderResponse> Products, decimal Total, Guid AddressId, DeliveryStatus Status,
		DateTime? OrderedAt, DateTime? SendAt, List<Guid> PaymentIdList);
	public record ProductInOrderResponse(int Id, string Name, decimal UnitPrice, int Quantity);
}