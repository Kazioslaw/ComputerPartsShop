using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerRepository : ICRUDRepository<Customer, Guid>
	{
		public List<Customer> GetList()
		{
			throw new NotImplementedException();
		}

		public Customer Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public Customer Create(Customer request)
		{
			throw new NotImplementedException();
		}

		public Customer Update(Guid id, Customer request)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
		}
	}
}
