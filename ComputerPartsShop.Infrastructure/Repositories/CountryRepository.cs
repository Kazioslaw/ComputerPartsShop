using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CountryRepository : ICountryRepository
	{
		private readonly TempData _dbContext;

		public CountryRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Country>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);

			return _dbContext.CountryList;
		}

		public async Task<Country> GetAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var country = _dbContext.CountryList.FirstOrDefault(c => c.Id == id);

			return country!;
		}

		public async Task<Country> GetByCountry3CodeAsync(string Country3Code, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var country = _dbContext.CountryList.FirstOrDefault(x => x.Alpha3 == Country3Code);

			return country!;
		}

		public async Task<int> CreateAsync(Country request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var last = _dbContext.CountryList.OrderBy(x => x.Id).LastOrDefault();

			if (last == null)
			{
				request.Id = 1;
			}
			else
			{
				request.Id = last.Id + 1;
			}

			_dbContext.CountryList.Add(request);

			return request.Id;
		}

		public async Task<Country> UpdateAsync(int id, Country request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var country = _dbContext.CountryList.FirstOrDefault(x => x.Id == id);

			if (country != null)
			{
				country.Alpha2 = request.Alpha2;
				country.Alpha3 = request.Alpha3;
				country.Name = request.Name;
			}

			return request;
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var country = _dbContext.CountryList.FirstOrDefault(x => x.Id == id);

			if (country != null)
			{
				_dbContext.CountryList.Remove(country);
			}
		}
	}
}
