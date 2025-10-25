using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IAddressService
	{
		public Task<List<AddressResponse>> GetListAsync(string username, CancellationToken ct);
		public Task<DetailedAddressResponse> GetAsync(Guid id, string username, CancellationToken ct);
		public Task<AddressResponse> CreateAsync(AddressRequest request, CancellationToken ct);
		public Task<AddressResponse> UpdateAsync(Guid id, UpdateAddressRequest request, CancellationToken ct);
		public Task DeleteAsync(Guid id, string username, CancellationToken ct);
	}
}
