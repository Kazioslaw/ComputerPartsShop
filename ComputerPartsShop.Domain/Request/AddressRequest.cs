namespace ComputerPartsShop.Domain.DTO
{
	public record AddressRequest(string Street, string City, string Region, string ZipCode, string Country3Code, string? Username, string? Email);
}