using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerPaymentSystemRepository : ICRUDRepository<CustomerPaymentSystem, Guid>
	{
		readonly TempData _dbContext;

		public CustomerPaymentSystemRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<CustomerPaymentSystem>> GetList()
		{
			return _dbContext.CustomerPaymentSystemList;
		}

		public async Task<CustomerPaymentSystem> Get(Guid id)
		{
			var cps = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.ID == id);

			return cps;
		}

		public async Task<Guid> Create(CustomerPaymentSystem request)
		{
			request.ID = Guid.NewGuid();

			_dbContext.CustomerPaymentSystemList.Add(request);

			return request.ID;
		}

		public async Task<CustomerPaymentSystem> Update(Guid id, CustomerPaymentSystem request)
		{
			var cps = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.ID == id);

			if (cps != null)
			{
				cps.CustomerID = request.CustomerID;
				cps.ProviderID = request.ProviderID;
				cps.PaymentReference = request.PaymentReference;
			}

			return cps;
		}

		public async Task Delete(Guid id)
		{
			var cps = _dbContext.CustomerPaymentSystemList.FirstOrDefault(x => x.ID == id);

			if (cps != null)
			{
				_dbContext.CustomerPaymentSystemList.Remove(cps);
			}
		}
	}
}
