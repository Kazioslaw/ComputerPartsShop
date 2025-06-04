namespace ComputerPartsShop.Domain.DTO
{
	public record AddressResponse(Guid Id, string Street, string City, string Region, string ZipCode, string Country3Code);
	public record DetailedAddressResponse(Guid Id, string Street, string City, string Region, string ZipCode, string Country3Code, List<UserInAddressResponse> users);
	public record UserInAddressResponse(string FirstName, string LastName, string? Username, string? Email);

}