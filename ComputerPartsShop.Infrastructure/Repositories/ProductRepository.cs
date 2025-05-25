using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class ProductRepository : ICRUDRepository<Product, int>
	{
		public Task<List<Product>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<Product> Get(int id)
		{
			throw new NotImplementedException();
		}

		public Task<int> Create(Product request)
		{
			throw new NotImplementedException();
		}

		public Task<Product> Update(int id, Product request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			return Task.CompletedTask;
		}
	}
}
