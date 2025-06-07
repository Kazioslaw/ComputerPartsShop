using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IAddressRepository
	{
		public Task<List<Address>> GetListAsync(string username, CancellationToken ct);
		public Task<Address> GetAsync(Guid id, string username, CancellationToken ct);
		public Task<Guid> GetAddressIDByFullDataAsync(Address request, CancellationToken ct);
		public Task<Address> CreateAsync(Address addressRequest, ShopUser userRequest, CancellationToken ct);
		public Task<Address> UpdateAsync(Guid id, Address address, Guid oldUserID, Guid newUserID, CancellationToken ct);
		public Task<int> DeleteAsync(Guid id, string username, CancellationToken ct);
	}
}
