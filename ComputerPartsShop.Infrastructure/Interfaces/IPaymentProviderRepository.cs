using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IPaymentProviderRepository
	{
		public Task<List<PaymentProvider>> GetListAsync(CancellationToken ct);
		public Task<PaymentProvider> GetAsync(int id, CancellationToken ct);
		public Task<PaymentProvider> GetByNameAsync(string input, CancellationToken ct);
		public Task<PaymentProvider> CreateAsync(PaymentProvider paymentProvider, CancellationToken ct);
		public Task<PaymentProvider> UpdateAsync(int id, PaymentProvider paymentProvider, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);
	}
}
