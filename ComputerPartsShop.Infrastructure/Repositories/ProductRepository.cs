using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class ProductRepository : ICRUDRepository<Product, int>
	{
		public List<Product> GetList()
		{
			throw new NotImplementedException();
		}

		public Product Get(int id)
		{
			throw new NotImplementedException();
		}

		public Product Create(Product request)
		{
			throw new NotImplementedException();
		}

		public Product Update(int id, Product request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{

		}
	}
}
