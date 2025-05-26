namespace ComputerPartsShop.Domain.DTO
{
	public record OrderRequest(string? Username, string? Email, List<int> ProductIdList, decimal Total, Guid AddressId, string? Status, DateTime? OrderedAt, DateTime? SendAt, Guid CustomerPaymentSystemId);
}