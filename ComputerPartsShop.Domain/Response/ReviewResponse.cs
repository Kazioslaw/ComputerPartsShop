namespace ComputerPartsShop.Domain.DTO
{
	public record ReviewResponse(int Id, string? Username, string ProductName, byte Rating, string? Description);
}