namespace ComputerPartsShop.Domain.DTO
{
	public record ShopUserRequest(string FirstName, string LastName, string Username, string Email, string? PhoneNumber, string Password);
	public record LoginRequest(string? Username, string? Email, string Password);
}