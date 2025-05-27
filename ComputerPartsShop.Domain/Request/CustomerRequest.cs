namespace ComputerPartsShop.Domain.DTO
{
	public record CustomerRequest(string FirstName, string LastName, string Username, string Email, string? PhoneNumber);
}