using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;

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
			try
			{
				var result = await _countryRepository.GetListAsync(ct);

				var countryList = _mapper.Map<IEnumerable<CountryResponse>>(result);

				return countryList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<DetailedCountryResponse> GetAsync(int id, CancellationToken ct)
		{
			try
			{
				var result = await _countryRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "Country not found");
				}

				var country = _mapper.Map<DetailedCountryResponse>(result);

				return country;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<CountryResponse> GetByAlpha3Async(string Alpha3, CancellationToken ct)
		{
			try
			{
				var result = await _countryRepository.GetByCountry3CodeAsync(Alpha3, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "Country not found");
				}

				var country = _mapper.Map<CountryResponse>(result);

				return country;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<CountryResponse> CreateAsync(CountryRequest entity, CancellationToken ct)
		{
			try
			{
				var newCountry = _mapper.Map<Country>(entity);
				var result = await _countryRepository.CreateAsync(newCountry, ct);
				var createdCountry = _mapper.Map<CountryResponse>(result);

				return createdCountry;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<CountryResponse> UpdateAsync(int id, CountryRequest entity, CancellationToken ct)
		{
			try
			{
				var existingCountry = await _countryRepository.GetAsync(id, ct);

				if (existingCountry == null)
				{
					throw new DataErrorException(404, "Country not found");
				}

				var countryToUpdate = new Country()
				{
					Alpha2 = entity.Alpha2,
					Alpha3 = entity.Alpha3,
					Name = entity.Name
				};

				var result = await _countryRepository.UpdateAsync(id, countryToUpdate, ct);

				var updatedCountry = _mapper.Map<CountryResponse>(result);

				return updatedCountry;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			try
			{
				var country = await _countryRepository.GetAsync(id, ct);

				if (country == null)
				{
					throw new DataErrorException(404, "Country not found");
				}

				await _countryRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}
	}
}
