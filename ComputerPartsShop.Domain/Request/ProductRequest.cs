namespace ComputerPartsShop.Domain.DTO
{
	public record ProductRequest(string Name, string Description, decimal UnitPrice, int Stock, string CategoryName, string InternalCode);
}