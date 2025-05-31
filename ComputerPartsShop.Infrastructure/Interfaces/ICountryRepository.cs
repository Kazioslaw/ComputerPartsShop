using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICountryRepository
	{
		public Task<List<Country>> GetListAsync(CancellationToken ct);
		public Task<Country> GetAsync(int id, CancellationToken ct);
		public Task<Country> GetByCountry3CodeAsync(string Country3Code, CancellationToken ct);
		public Task<Country> CreateAsync(Country country, CancellationToken ct);
		public Task<Country> UpdateAsync(int id, Country country, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);

	}
}
