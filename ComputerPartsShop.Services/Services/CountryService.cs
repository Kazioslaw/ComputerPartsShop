using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CountryService : ICountryService
	{
		private readonly ICountryRepository _countryRepository;
		private readonly IMapper _mapper;

		public CountryService(ICountryRepository countryRepository, IMapper mapper)
		{
			_countryRepository = countryRepository;
			_mapper = mapper;
		}

		public async Task<List<CountryResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _countryRepository.GetListAsync(ct);

			var countryList = _mapper.Map<IEnumerable<CountryResponse>>(result);

			return countryList.ToList();


		}

		public async Task<DetailedCountryResponse> GetAsync(int id, CancellationToken ct)
		{
			var result = await _countryRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var country = _mapper.Map<DetailedCountryResponse>(result);

			return country;
		}

		public async Task<CountryResponse> GetByAlpha3Async(string Alpha3, CancellationToken ct)
		{
			var result = await _countryRepository.GetByCountry3CodeAsync(Alpha3, ct);

			if (result == null)
			{
				return null;
			}

			var country = _mapper.Map<CountryResponse>(result);

			return country;
		}

		public async Task<CountryResponse> CreateAsync(CountryRequest entity, CancellationToken ct)
		{
			var newCountry = _mapper.Map<Country>(entity);
			var result = await _countryRepository.CreateAsync(newCountry, ct);

			if (result == null)
			{
				return null;
			}

			var createdCountry = _mapper.Map<CountryResponse>(result);

			return createdCountry;
		}

		public async Task<CountryResponse> UpdateAsync(int id, CountryRequest entity, CancellationToken ct)
		{
			var countryToUpdate = new Country()
			{
				Alpha2 = entity.Alpha2,
				Alpha3 = entity.Alpha3,
				Name = entity.Name
			};

			var result = await _countryRepository.UpdateAsync(id, countryToUpdate, ct);

			if (result == null)
			{
				return null;
			}

			var updatedCountry = _mapper.Map<CountryResponse>(result);

			return updatedCountry;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _countryRepository.DeleteAsync(id, ct);
		}
	}
}
