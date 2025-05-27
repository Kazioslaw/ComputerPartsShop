using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICountryRepository : IRepository<Country, int>
	{
		public Task<Country> GetByCountry3CodeAsync(string Country3Code, CancellationToken ct);
	}
}
