using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentProviderRepository : ICRUDRepository<PaymentProvider, int>
	{
		readonly TempData _dbContext;

		public PaymentProviderRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<PaymentProvider>> GetList()
		{
			return _dbContext.PaymentProviderList;
		}

		public async Task<PaymentProvider> Get(int id)
		{
			var paymentProvider = _dbContext.PaymentProviderList.FirstOrDefault(x => x.ID == id);

			return paymentProvider;
		}

		public async Task<PaymentProvider> GetByName(string input)
		{
			var paymentProvider = _dbContext.PaymentProviderList.FirstOrDefault(x => x.Name == input);

			return paymentProvider;
		}

		public async Task<int> Create(PaymentProvider request)
		{
			var last = _dbContext.PaymentProviderList.OrderBy(x => x.ID).FirstOrDefault();

			if (last == null)
			{
				request.ID = 1;
			}
			else
			{
				request.ID = last.ID + 1;
			}

			_dbContext.PaymentProviderList.Add(request);

			return request.ID;
		}

		public async Task<PaymentProvider> Update(int id, PaymentProvider request)
		{
			var paymentProvider = _dbContext.PaymentProviderList.FirstOrDefault(x => x.ID == id);

			if (paymentProvider != null)
			{
				paymentProvider.Name = request.Name;
			}

			return paymentProvider;
		}

		public async Task Delete(int id)
		{
			var paymentProvider = _dbContext.PaymentProviderList.FirstOrDefault(x => x.ID == id);

			if (paymentProvider != null)
			{
				_dbContext.PaymentProviderList.Remove(paymentProvider);
			}
		}
	}
}
