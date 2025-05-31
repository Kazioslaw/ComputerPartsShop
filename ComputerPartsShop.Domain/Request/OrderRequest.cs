using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.DTO
{
	public record OrderRequest(string? Username, string? Email, List<ProductIdWithQuantityRequest> Products, decimal Total, Guid AddressId, DeliveryStatus? Status, DateTime? OrderedAt, DateTime? SendAt, Guid CustomerPaymentSystemId);
	public record ProductIdWithQuantityRequest(int Id, int Quantity);
}