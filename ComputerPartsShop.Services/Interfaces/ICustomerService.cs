using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICustomerService
	{
		public Task<List<CustomerResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedCustomerResponse> GetAsync(Guid id, CancellationToken ct);
		public Task<CustomerResponse> GetByUsernameOrEmailAsync(string input, CancellationToken ct);
		public Task<CustomerResponse> CreateAsync(CustomerRequest request, CancellationToken ct);
		public Task<CustomerResponse> UpdateAsync(Guid id, CustomerRequest request, CancellationToken ct);
		public Task<bool> DeleteAsync(Guid id, CancellationToken ct);
	}
}
