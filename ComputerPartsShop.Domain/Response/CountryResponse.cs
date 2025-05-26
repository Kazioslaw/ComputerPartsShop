namespace ComputerPartsShop.Domain.DTO
{
	public record CountryResponse(int Id, string Alpha2, string Alpha3, string Name);
	public record DetailedCountryResponse(int Id, string Alpha2, string Alpha3, string Name, List<AddressInCountryResponse> AddressList);
	public record AddressInCountryResponse(Guid Id, string Street, string City, string Region, string ZipCode);
}