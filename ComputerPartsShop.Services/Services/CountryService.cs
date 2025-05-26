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
			var addressList = country.Addresses;

			return country == null ? null! : new DetailedCountryResponse(country.Id, country.Alpha2, country.Alpha3, country.Name,
				addressList.Select(a => new AddressInCountryResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode)).ToList());
		}

		public async Task<CountryResponse> CreateAsync(CountryRequest entity, CancellationToken ct)
		{
			var newCountry = new Country()
			{
				Alpha2 = entity.Alpha2,
				Alpha3 = entity.Alpha3,
				Name = entity.Name,
			};

			var createdCountryId = await _countryRepository.CreateAsync(newCountry, ct);
			return new CountryResponse(createdCountryId, entity.Alpha2, entity.Alpha3, entity.Name);
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

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await _countryRepository.DeleteAsync(id, ct);
		}
	}
}
