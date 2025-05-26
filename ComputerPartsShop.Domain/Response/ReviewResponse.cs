namespace ComputerPartsShop.Domain.DTO
{
	public record ReviewResponse(int ID, string? Username, string ProductName, byte Rating, string? Description);
}