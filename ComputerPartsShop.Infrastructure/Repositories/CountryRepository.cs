using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CountryRepository : ICRUDRepository<Country, int>
	{
		public Task<List<Country>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<Country> Get(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Country> GetByCountry3Code(string Country3Code)
		{
			throw new NotImplementedException();
		}

		public Task<int> Create(Country request)
		{
			throw new NotImplementedException();
		}

		public Task<Country> Update(int id, Country request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			return Task.CompletedTask;
		}
	}
}
