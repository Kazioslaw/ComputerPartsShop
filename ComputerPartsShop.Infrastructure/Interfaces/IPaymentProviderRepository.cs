using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IPaymentProviderRepository : IRepository<PaymentProvider, int>
	{
		public Task<PaymentProvider> GetByName(string input);
	}
}
