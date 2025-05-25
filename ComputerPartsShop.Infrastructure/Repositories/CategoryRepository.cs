using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CategoryRepository : ICRUDRepository<Category, int>
	{
		public Task<List<Category>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<Category> Get(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Category> GetByName(string name)
		{
			throw new NotImplementedException();
		}

		public Task<int> Create(Category request)
		{
			throw new NotImplementedException();
		}

		public Task<Category> Update(int id, Category request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			return Task.CompletedTask;
		}
	}
}
