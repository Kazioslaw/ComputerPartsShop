using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IAddressRepository
	{
		public Task<List<Address>> GetListAsync(CancellationToken ct);
		public Task<Address> GetAsync(Guid id, CancellationToken ct);
		public Task<Guid> GetAddressIDByFullDataAsync(Address request, CancellationToken ct);
		public Task<Address> CreateAsync(Address addressRequest, ShopUser userRequest, CancellationToken ct);
		public Task<Address> UpdateAsync(Guid id, Address address, Guid oldUserID, Guid newUserID, CancellationToken ct);
		public Task DeleteAsync(Guid id, CancellationToken ct);
	}
}
