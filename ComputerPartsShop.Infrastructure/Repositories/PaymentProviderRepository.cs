using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentProviderRepository : ICRUDRepository<PaymentProvider, int>
	{
		public Task<List<PaymentProvider>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<PaymentProvider> Get(int id)
		{
			throw new NotImplementedException();
		}

		public Task<PaymentProvider> GetByName(string input)
		{
			throw new NotImplementedException();
		}

		public Task<int> Create(PaymentProvider request)
		{
			throw new NotImplementedException();
		}

		public Task<PaymentProvider> Update(int id, PaymentProvider request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			return Task.CompletedTask;
		}
	}
}
