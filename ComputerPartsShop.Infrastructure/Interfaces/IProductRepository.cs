using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IProductRepository
	{
		public Task<List<Product>> GetListAsync(CancellationToken ct);
		public Task<Product> GetAsync(int id, CancellationToken ct);
		public Task<Product> CreateAsync(Product product, CancellationToken ct);
		public Task<Product> UpdateAsync(int id, Product product, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);
	}
}
