namespace ComputerPartsShop.Domain.DTO
{
	public record ReviewRequest(string? Username, int ProductId, byte Rating, string? Description);
}