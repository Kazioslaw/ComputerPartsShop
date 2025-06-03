using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICategoryService
	{
		public Task<List<CategoryResponse>> GetListAsync(CancellationToken ct);
		public Task<CategoryResponse> GetAsync(int id, CancellationToken ct);
		public Task<CategoryResponse> GetByNameAsync(string name, CancellationToken ct);
		public Task<CategoryResponse> CreateAsync(CategoryRequest request, CancellationToken ct);
		public Task<CategoryResponse> UpdateAsync(int id, CategoryRequest request, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);

	}
}
