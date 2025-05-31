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

		public async Task<List<CountryResponse>> GetListAsync(CancellationToken ct)
		{
			var countryList = await _countryRepository.GetListAsync(ct);

			return countryList.Select(c => new CountryResponse(c.Id, c.Alpha2, c.Alpha3, c.Name)).ToList();


		}

		public async Task<DetailedCountryResponse> GetAsync(int id, CancellationToken ct)
		{
			var country = await _countryRepository.GetAsync(id, ct);

			if (country == null)
			{
				return null;
			}

			var addressList = country.Addresses;

			return new DetailedCountryResponse(country.Id, country.Alpha2, country.Alpha3, country.Name, addressList == null ?
				country.Addresses.Select(x => new AddressInCountryResponse(x.Id, x.Street, x.City, x.Region, x.ZipCode)).ToList() : new List<AddressInCountryResponse>());
		}

		public async Task<CountryResponse> GetByAlpha3Async(string Alpha3, CancellationToken ct)
		{
			var country = await _countryRepository.GetByCountry3CodeAsync(Alpha3, ct);

			if (country == null)
			{
				return null;
			}

			return new CountryResponse(country.Id, country.Alpha2, country.Alpha3, country.Name);
		}

		public async Task<CountryResponse> CreateAsync(CountryRequest entity, CancellationToken ct)
		{
			var newCountry = new Country()
			{
				Alpha2 = entity.Alpha2,
				Alpha3 = entity.Alpha3,
				Name = entity.Name,
			};

			var createdCountry = await _countryRepository.CreateAsync(newCountry, ct);
			return createdCountry == null ? null! : new CountryResponse(createdCountry.Id, entity.Alpha2, entity.Alpha3, entity.Name);
		}

		public async Task<CountryResponse> UpdateAsync(int id, CountryRequest entity, CancellationToken ct)
		{
			var country = new Country()
			{
				Alpha2 = entity.Alpha2,
				Alpha3 = entity.Alpha3,
				Name = entity.Name
			};

			await _countryRepository.UpdateAsync(id, country, ct);

			return new CountryResponse(id, entity.Alpha2, entity.Alpha3, entity.Name);
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _countryRepository.DeleteAsync(id, ct);
		}
	}
}
