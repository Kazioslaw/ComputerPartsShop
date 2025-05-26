using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICustomerService
	{
		public Task<List<CustomerResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedCustomerResponse> GetAsync(Guid id, CancellationToken ct);
		public Task<CustomerResponse> CreateAsync(CustomerRequest entity, CancellationToken ct);
		public Task<CustomerResponse> UpdateAsync(Guid id, CustomerRequest entity, CancellationToken ct);
		public Task DeleteAsync(Guid id, CancellationToken ct);
	}
}
