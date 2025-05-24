using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerPaymentSystemRepository : ICRUDRepository<CustomerPaymentSystem, Guid>
	{
		public List<CustomerPaymentSystem> GetList()
		{
			throw new NotImplementedException();
		}

		public CustomerPaymentSystem Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public CustomerPaymentSystem Create(CustomerPaymentSystem request)
		{
			throw new NotImplementedException();
		}

		public CustomerPaymentSystem Update(Guid id, CustomerPaymentSystem request)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{

		}
	}
}
