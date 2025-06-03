using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICustomerPaymentSystemRepository
	{
		public Task<List<CustomerPaymentSystem>> GetListAsync(CancellationToken ct);
		public Task<CustomerPaymentSystem> GetAsync(Guid id, CancellationToken ct);
		public Task<CustomerPaymentSystem> CreateAsync(CustomerPaymentSystem customerPaymentSystem, CancellationToken ct);
		public Task<CustomerPaymentSystem> UpdateAsync(Guid id, CustomerPaymentSystem customerPaymentSystem, CancellationToken ct);
		public Task<bool> DeleteAsync(Guid id, CancellationToken ct);
	}
}