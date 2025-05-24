namespace ComputerPartsShop.Domain.DTOs
{
	public record ProductRequest(string Name, string Description, decimal UnitPrice, int Stock, string CategoryName, string InternalCode);
	public record ProductResponse(int ID, string Name, string Description, string UnitPrice, int Stock, string CategoryName, string InternalCode);
}
