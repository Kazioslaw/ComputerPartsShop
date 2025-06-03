using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICustomerRepository
	{
		public Task<List<Customer>> GetListAsync(CancellationToken ct);
		public Task<Customer> GetAsync(Guid id, CancellationToken ct);
		public Task<Customer> GetByUsernameOrEmailAsync(string input, CancellationToken ct);
		public Task<Customer> GetByAddressIDAsync(Guid addressID, CancellationToken ct);
		public Task<Customer> CreateAsync(Customer customer, CancellationToken ct);
		public Task<Customer> UpdateAsync(Guid id, Customer customer, CancellationToken ct);
		public Task<bool> DeleteAsync(Guid id, CancellationToken ct);

	}
}
