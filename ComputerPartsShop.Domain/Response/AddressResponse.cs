namespace ComputerPartsShop.Domain.DTO
{
	public record AddressResponse(Guid ID, string Street, string City, string Region, string ZipCode, string Country3Code);
}