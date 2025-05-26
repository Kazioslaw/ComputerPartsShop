namespace ComputerPartsShop.Domain.DTO
{
	public record ReviewRequest(string? Username, int ProductID, byte Rating, string? Description);
}