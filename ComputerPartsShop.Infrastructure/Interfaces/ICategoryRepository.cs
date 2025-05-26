using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICategoryRepository : IRepository<Category, int>
	{
		public Task<Category> GetByNameAsync(string name);
	}
}
