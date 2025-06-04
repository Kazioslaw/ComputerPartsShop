using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICountryRepository
	{
		public Task<List<Country>> GetListAsync(CancellationToken ct);
		public Task<Country> GetAsync(int id, CancellationToken ct);
		public Task<Country> GetByCountry3CodeAsync(string Country3Code, CancellationToken ct);
		public Task<Country> CreateAsync(Country request, CancellationToken ct);
		public Task<Country> UpdateAsync(int id, Country request, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);

	}
}
