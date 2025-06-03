using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IPaymentProviderService
	{
		public Task<List<PaymentProviderResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedPaymentProviderResponse> GetAsync(int id, CancellationToken ct);
		public Task<DetailedPaymentProviderResponse> GetByNameAsync(string name, CancellationToken ct);
		public Task<PaymentProviderResponse> CreateAsync(PaymentProviderRequest request, CancellationToken ct);
		public Task<PaymentProviderResponse> UpdateAsync(int id, PaymentProviderRequest request, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);
	}
}
