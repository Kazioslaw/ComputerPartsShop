using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICountryRepository : IRepository<Country, int>
	{
		public Task<Country> GetByCountry3Code(string Country3Code);
	}
}
