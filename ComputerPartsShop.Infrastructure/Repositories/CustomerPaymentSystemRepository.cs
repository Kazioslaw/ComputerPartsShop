using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerPaymentSystemRepository : ICustomerPaymentSystemRepository
	{
		readonly TempData _dbContext;

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
			var cps = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.ID == id);

			return cps!;
		}

		public async Task<Guid> CreateAsync(CustomerPaymentSystem request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			request.ID = Guid.NewGuid();

			_dbContext.CustomerPaymentSystemList.Add(request);

			return request.ID;
		}

		public async Task<CustomerPaymentSystem> UpdateAsync(Guid id, CustomerPaymentSystem request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var cps = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.ID == id);

			if (cps != null)
			{
				cps.CustomerID = request.CustomerID;
				cps.ProviderID = request.ProviderID;
				cps.PaymentReference = request.PaymentReference;
			}

			return cps!;
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var cps = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.ID == id);

			if (cps != null)
			{
				_dbContext.CustomerPaymentSystemList.Remove(cps);
			}
		}
	}
}
