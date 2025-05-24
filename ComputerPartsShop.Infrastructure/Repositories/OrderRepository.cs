using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class OrderRepository : ICRUDRepository<Order, int>
	{
		public List<Order> GetList()
		{
			throw new NotImplementedException();
		}

		public Order Get(int id)
		{
			throw new NotImplementedException();
		}

		public Order Create(Order request)
		{
			throw new NotImplementedException();
		}

		public Order Update(int id, Order request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{

		}
	}
}
