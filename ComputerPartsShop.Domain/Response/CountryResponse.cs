namespace ComputerPartsShop.Domain.DTO
{
	public record CountryResponse(int ID, string Alpha2, string Alpha3, string Name);
	public record DetailedCountryResponse(int ID, string Alpha2, string Alpha3, string Name, List<AddressInCountryResponse> AddressList);
	public record AddressInCountryResponse(Guid ID, string Street, string City, string Region, string ZipCode);
}