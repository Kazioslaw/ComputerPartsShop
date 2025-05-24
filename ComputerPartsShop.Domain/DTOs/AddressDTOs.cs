namespace ComputerPartsShop.Domain.DTOs
{
	public record AddressRequest(string Street, string City, string Region, string ZipCode, string Country3Code, string? Username, string? Email);
	public record AddressResponse(Guid ID, string Street, string City, string Region, string ZipCode, string Country3Code);
}