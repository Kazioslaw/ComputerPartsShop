using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IProductRepository : IRepository<Product, int>
	{
	}
}
