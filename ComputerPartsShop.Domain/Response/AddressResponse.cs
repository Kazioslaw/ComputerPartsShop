namespace ComputerPartsShop.Domain.DTO
{
	public record AddressResponse(Guid Id, string Street, string City, string Region, string ZipCode, string Country3Code);

	public record DetailedAddressresponse(Guid Id, string Street, string City, string Region, string ZipCode, string Country3Code, string Username, string Email);
}