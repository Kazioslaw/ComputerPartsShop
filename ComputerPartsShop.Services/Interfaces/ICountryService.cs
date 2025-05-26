using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICountryService : IService<CountryRequest, CountryResponse, DetailedCountryResponse, int>
	{
	}
}
