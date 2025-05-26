using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CountryService : ICountryService
	{
		private readonly ICountryRepository _countryRepository;

		public CountryService(ICountryRepository countryRepository)
		{
			_countryRepository = countryRepository;
		}

		public async Task<List<CountryResponse>> GetListAsync()
		{
			var countryList = await _countryRepository.GetListAsync();

			return countryList.Select(c => new CountryResponse(c.ID, c.Alpha2, c.Alpha3, c.Name)).ToList();


		}

		public async Task<DetailedCountryResponse> GetAsync(int id)
		{
			var country = await _countryRepository.GetAsync(id);
			var addressList = country.Addresses;

			return country == null ? null! : new DetailedCountryResponse(country.ID, country.Alpha2, country.Alpha3, country.Name,
				addressList.Select(a => new AddressInCountryResponse(a.ID, a.Street, a.City, a.Region, a.ZipCode)).ToList());
		}

		public async Task<CountryResponse> CreateAsync(CountryRequest country)
		{
			var newCountry = new Country()
			{
				Alpha2 = country.Alpha2,
				Alpha3 = country.Alpha3,
				Name = country.Name,
			};

			var createdCountryID = await _countryRepository.CreateAsync(newCountry);
			return new CountryResponse(createdCountryID, country.Alpha2, country.Alpha3, country.Name);
		}

		public async Task<CountryResponse> UpdateAsync(int id, CountryRequest updatedCountry)
		{
			var country = new Country()
			{
				Alpha2 = updatedCountry.Alpha2,
				Alpha3 = updatedCountry.Alpha3,
				Name = updatedCountry.Name
			};

			await _countryRepository.UpdateAsync(id, country);

			return new CountryResponse(id, updatedCountry.Alpha2, updatedCountry.Alpha3, updatedCountry.Name);
		}

		public async Task DeleteAsync(int id)
		{
			await _countryRepository.DeleteAsync(id);
		}

	}
}
