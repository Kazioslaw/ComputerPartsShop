using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerPaymentSystemRepository : ICRUDRepository<CustomerPaymentSystem, Guid>
	{
		public Task<List<CustomerPaymentSystem>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<CustomerPaymentSystem> Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<Guid> Create(CustomerPaymentSystem request)
		{
			throw new NotImplementedException();
		}

		public Task<CustomerPaymentSystem> Update(Guid id, CustomerPaymentSystem request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(Guid id)
		{
			return Task.CompletedTask;
		}
	}
}
