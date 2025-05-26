using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IPaymentProviderService
	{
		public Task<List<PaymentProviderResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedPaymentProviderResponse> GetAsync(int id, CancellationToken ct);
		public Task<PaymentProviderResponse> CreateAsync(PaymentProviderRequest entity, CancellationToken ct);
		public Task<PaymentProviderResponse> UpdateAsync(int id, PaymentProviderRequest entity, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);
	}
}
