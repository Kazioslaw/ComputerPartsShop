using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICountryService
	{
		public Task<List<CountryResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedCountryResponse> GetAsync(int id, CancellationToken ct);
		public Task<CountryResponse> CreateAsync(CountryRequest entity, CancellationToken ct);
		public Task<CountryResponse> UpdateAsync(int id, CountryRequest entity, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);
	}
}
