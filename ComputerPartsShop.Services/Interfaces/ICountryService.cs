using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICountryService
	{
		public Task<List<CountryResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedCountryResponse> GetAsync(int id, CancellationToken ct);
		public Task<CountryResponse> GetByAlpha3Async(string alpha3, CancellationToken ct);
		public Task<CountryResponse> CreateAsync(CountryRequest request, CancellationToken ct);
		public Task<CountryResponse> UpdateAsync(int id, CountryRequest request, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);
	}
}
