namespace ComputerPartsShop.Domain.DTO
{
	public record AddressResponse(Guid Id, string Street, string City, string Region, string ZipCode, string Country3Code);
	public record DetailedAddressResponse(Guid Id, string Street, string City, string Region, string ZipCode, string Country3Code, List<CustomerInAddressResponse> customers);
	public record CustomerInAddressResponse(string FirstName, string LastName, string? Username, string? Email);

}