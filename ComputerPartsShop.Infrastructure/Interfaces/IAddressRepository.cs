using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IAddressRepository
	{
		public Task<List<Address>> GetListAsync(CancellationToken ct);
		public Task<Address> GetAsync(Guid id, CancellationToken ct);
		public Task<Guid> GetAddressIDByFullDataAsync(Address address, CancellationToken ct);
		public Task<Address> CreateAsync(Address address, Customer customer, CancellationToken ct);
		public Task<Address> UpdateAsync(Guid id, Address address, Guid oldCustomerID, Guid newCustomerID, CancellationToken ct);
		public Task<bool> DeleteAsync(Guid id, CancellationToken ct);
	}
}
