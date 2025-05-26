using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentProviderRepository : IPaymentProviderRepository
	{
		readonly TempData _dbContext;

		public PaymentProviderRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<PaymentProvider>> GetListAsync()
		{
			return _dbContext.PaymentProviderList;
		}

		public async Task<PaymentProvider> GetAsync(int id)
		{
			var paymentProvider = _dbContext.PaymentProviderList.FirstOrDefault(x => x.ID == id);

			return paymentProvider;
		}

		public async Task<PaymentProvider> GetByNameAsync(string input)
		{
			var paymentProvider = _dbContext.PaymentProviderList.FirstOrDefault(x => x.Name == input);

			return paymentProvider;
		}

		public async Task<int> CreateAsync(PaymentProvider request)
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

		public async Task<PaymentProvider> UpdateAsync(int id, PaymentProvider request)
		{
			var paymentProvider = _dbContext.PaymentProviderList.FirstOrDefault(x => x.ID == id);

			if (paymentProvider != null)
			{
				paymentProvider.Name = request.Name;
			}

			return paymentProvider;
		}

		public async Task DeleteAsync(int id)
		{
			var paymentProvider = _dbContext.PaymentProviderList.FirstOrDefault(x => x.ID == id);

			if (paymentProvider != null)
			{
				_dbContext.PaymentProviderList.Remove(paymentProvider);
			}
		}
	}
}
