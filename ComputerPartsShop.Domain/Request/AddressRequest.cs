namespace ComputerPartsShop.Domain.DTO
{
	public record AddressRequest(string Street, string City, string Region, string ZipCode, string Country3Code, string? Username, string? Email);

	public record UpdateAddressRequest(string newStreet, string newCity, string newRegion, string newZipCode, string newCountry3Code,
		string? oldUsername, string? oldEmail, string? newUsername, string? newEmail);
}