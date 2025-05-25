using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerRepository : ICRUDRepository<Customer, Guid>
	{
		public Task<List<Customer>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<Customer> Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<Customer> GetByUsernameOrEmail(string input)
		{
			throw new NotImplementedException();
		}

		public Task<Guid> Create(Customer request)
		{
			throw new NotImplementedException();
		}

		public Task<Customer> Update(Guid id, Customer request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(Guid id)
		{
			return Task.CompletedTask;
		}
	}
}
