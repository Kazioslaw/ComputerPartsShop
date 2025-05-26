namespace ComputerPartsShop.Domain.DTO
{
	public record ProductResponse(int Id, string Name, string Description, decimal UnitPrice, int Stock, string CategoryName, string InternalCode);
}