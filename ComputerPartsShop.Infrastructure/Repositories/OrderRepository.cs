using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class OrderRepository : ICRUDRepository<Order, int>
	{
		public Task<List<Order>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<Order> Get(int id)
		{
			throw new NotImplementedException();
		}

		public Task<int> Create(Order request)
		{
			throw new NotImplementedException();
		}

		public Task<Order> Update(int id, Order request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			return Task.CompletedTask;
		}
	}
}
