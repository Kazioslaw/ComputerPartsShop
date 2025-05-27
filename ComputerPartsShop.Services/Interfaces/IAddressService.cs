using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IAddressService
	{
		public Task<List<AddressResponse>> GetListAsync(CancellationToken ct);
		public Task<AddressResponse> GetAsync(Guid id, CancellationToken ct);
		public Task<AddressResponse> CreateAsync(AddressRequest entity, CancellationToken ct);
		public Task<AddressResponse> UpdateAsync(Guid id, AddressRequest entity, CancellationToken ct);
		public Task DeleteAsync(Guid id, CancellationToken ct);
	}
}
