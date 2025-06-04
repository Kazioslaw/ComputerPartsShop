using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICategoryRepository
	{
		public Task<List<Category>> GetListAsync(CancellationToken ct);
		public Task<Category> GetAsync(int id, CancellationToken ct);
		public Task<Category> GetByNameAsync(string name, CancellationToken ct);
		public Task<Category> CreateAsync(Category request, CancellationToken ct);
		public Task<Category> UpdateAsync(int id, Category request, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);

	}
}
