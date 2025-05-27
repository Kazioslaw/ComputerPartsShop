using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IPaymentProviderRepository : IRepository<PaymentProvider, int>
	{
		public Task<PaymentProvider> GetByNameAsync(string input, CancellationToken ct);
	}
}
