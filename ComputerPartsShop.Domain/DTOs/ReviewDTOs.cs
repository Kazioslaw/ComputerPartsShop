namespace ComputerPartsShop.Domain.DTOs
{
	public record ReviewRequest(string? Username, int ProductID, byte Rating, string? Description);
	public record ReviewResponse(int ID, string? Username, string ProductName, byte Rating, string? Description);
}
