using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CountryRepository : ICountryRepository
	{
		readonly TempData _dbContext;

		public CountryRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Country>> GetList()
		{
			return _dbContext.CountryList;
		}

		public async Task<Country> Get(int id)
		{
			var country = _dbContext.CountryList.FirstOrDefault(c => c.ID == id);

			return country;
		}

		public async Task<Country> GetByCountry3Code(string Country3Code)
		{
			var country = _dbContext.CountryList.FirstOrDefault(x => x.Alpha3 == Country3Code);

			return country;
		}

		public async Task<int> Create(Country request)
		{
			var last = _dbContext.CountryList.OrderBy(x => x.ID).LastOrDefault();

			if (last == null)
			{
				request.ID = 1;
			}
			else
			{
				request.ID = last.ID + 1;
			}

			_dbContext.CountryList.Add(request);

			return request.ID;
		}

		public async Task<Country> Update(int id, Country request)
		{
			var country = _dbContext.CountryList.FirstOrDefault(x => x.ID == id);

			if (country != null)
			{
				country.Alpha2 = request.Alpha2;
				country.Alpha3 = request.Alpha3;
				country.Name = request.Name;
			}

			return request;
		}

		public async Task Delete(int id)
		{
			var country = _dbContext.CountryList.FirstOrDefault(x => x.ID == id);

			if (country != null)
			{
				_dbContext.CountryList.Remove(country);
			}
		}
	}
}
