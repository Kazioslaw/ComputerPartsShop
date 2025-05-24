using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CategoryRepository : ICRUDRepository<Category, int>
	{
		public List<Category> GetList()
		{
			throw new NotImplementedException();
		}

		public Category Get(int id)
		{
			throw new NotImplementedException();
		}

		public Category Create(Category request)
		{
			throw new NotImplementedException();
		}

		public Category Update(int id, Category request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{

		}
	}
}
