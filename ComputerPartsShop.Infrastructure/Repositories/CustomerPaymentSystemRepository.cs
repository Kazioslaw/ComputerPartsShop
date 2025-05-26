using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerPaymentSystemRepository : ICustomerPaymentSystemRepository
	{
		private readonly TempData _dbContext;

		public CustomerPaymentSystemRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<CustomerPaymentSystem>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);

			return _dbContext.CustomerPaymentSystemList;
		}

		public async Task<CustomerPaymentSystem> GetAsync(Guid id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var customerPaymentSystem = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.Id == id);

			return customerPaymentSystem!;
		}

		public async Task<Guid> CreateAsync(CustomerPaymentSystem request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			request.Id = Guid.NewGuid();

			_dbContext.CustomerPaymentSystemList.Add(request);

			return request.Id;
		}

		public async Task<CustomerPaymentSystem> UpdateAsync(Guid id, CustomerPaymentSystem request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var customerPaymentSystem = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.Id == id);

			if (customerPaymentSystem != null)
			{
				customerPaymentSystem.CustomerId = request.CustomerId;
				customerPaymentSystem.ProviderId = request.ProviderId;
				customerPaymentSystem.PaymentReference = request.PaymentReference;
			}

			return customerPaymentSystem!;
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var customerPaymentSystem = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.Id == id);

			if (customerPaymentSystem != null)
			{
				_dbContext.CustomerPaymentSystemList.Remove(customerPaymentSystem);
			}
		}
	}
}
