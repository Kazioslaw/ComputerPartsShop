using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IProductService
	{
		public Task<List<ProductResponse>> GetListAsync(CancellationToken ct);
		public Task<ProductResponse> GetAsync(int id, CancellationToken ct);
		public Task<ProductResponse> CreateAsync(ProductRequest entity, CancellationToken ct);
		public Task<ProductResponse> UpdateAsync(int id, ProductRequest entity, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);
	}
}
