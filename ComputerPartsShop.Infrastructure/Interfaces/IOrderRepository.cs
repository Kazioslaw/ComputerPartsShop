using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IOrderRepository : IRepository<Order, int>
	{
	}
}
