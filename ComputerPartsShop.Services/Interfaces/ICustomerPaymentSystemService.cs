using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICustomerPaymentSystemService
	{
		public Task<List<CustomerPaymentSystemResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedCustomerPaymentSystemResponse> GetAsync(Guid id, CancellationToken ct);
		public Task<CustomerPaymentSystemResponse> CreateAsync(CustomerPaymentSystemRequest entity, CancellationToken ct);
		public Task<CustomerPaymentSystemResponse> UpdateAsync(Guid id, CustomerPaymentSystemRequest entity, CancellationToken ct);
		public Task DeleteAsync(Guid id, CancellationToken ct);
	}
}
