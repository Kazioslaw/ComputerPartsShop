using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICategoryRepository
	{
		public Task<List<Category>> GetListAsync(CancellationToken ct);
		public Task<Category> GetAsync(int id, CancellationToken ct);
		public Task<Category> GetByNameAsync(string name, CancellationToken ct);
		public Task<Category> CreateAsync(Category category, CancellationToken ct);
		public Task<Category> UpdateAsync(int id, Category category, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);

	}
}
