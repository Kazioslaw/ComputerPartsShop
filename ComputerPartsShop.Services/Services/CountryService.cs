using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CountryService : ICRUDService<CountryRequest, CountryResponse, DetailedCountryResponse, int>
	{
		private readonly CountryRepository _countryRepository;

		public CountryService(CountryRepository countryRepository)
		{
			_countryRepository = countryRepository;
		}

		public async Task<List<CountryResponse>> GetList()
		{
			var countryList = await _countryRepository.GetList();

			return countryList.Select(c => new CountryResponse(c.ID, c.Alpha2, c.Alpha3, c.Name)).ToList();


		}

		public async Task<DetailedCountryResponse> Get(int id)
		{
			var country = await _countryRepository.Get(id);
			var addressList = country.Addresses;

			return country == null ? null! : new DetailedCountryResponse(country.ID, country.Alpha2, country.Alpha3, country.Name,
				addressList.Select(a => new AddressInCountryResponse(a.ID, a.Street, a.City, a.Region, a.ZipCode)).ToList());
		}

		public async Task<CountryResponse> Create(CountryRequest country)
		{
			var newCountry = new Country()
			{
				Alpha2 = country.Alpha2,
				Alpha3 = country.Alpha3,
				Name = country.Name,
			};

			var createdCountryID = await _countryRepository.Create(newCountry);
			return new CountryResponse(createdCountryID, country.Alpha2, country.Alpha3, country.Name);
		}

		public async Task<CountryResponse> Update(int id, CountryRequest updatedCountry)
		{
			var country = new Country()
			{
				Alpha2 = updatedCountry.Alpha2,
				Alpha3 = updatedCountry.Alpha3,
				Name = updatedCountry.Name
			};

			await _countryRepository.Update(id, country);

			return new CountryResponse(id, updatedCountry.Alpha2, updatedCountry.Alpha3, updatedCountry.Name);
		}

		public async Task Delete(int id)
		{
			await _countryRepository.Delete(id);
		}

	}
}
