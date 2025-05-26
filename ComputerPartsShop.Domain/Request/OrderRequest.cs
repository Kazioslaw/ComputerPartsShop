namespace ComputerPartsShop.Domain.DTO
{
	public record OrderRequest(string? Username, string? Email, List<int> ProductIDList, decimal Total, Guid AddressID, string? Status, DateTime? OrderedAt, DateTime? SendAt, Guid CustomerPaymentSystemID);
}