using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICategoryService
	{
		public Task<List<CategoryResponse>> GetListAsync(CancellationToken ct);
		public Task<CategoryResponse> GetAsync(int id, CancellationToken ct);
		public Task<CategoryResponse> CreateAsync(CategoryRequest entity, CancellationToken ct);
		public Task<CategoryResponse> UpdateAsync(int id, CategoryRequest entity, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);
	}
}
