using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IReviewService
	{
		public Task<List<ReviewResponse>> GetListAsync(CancellationToken ct);
		public Task<ReviewResponse> GetAsync(int id, CancellationToken ct);
		public Task<ReviewResponse> CreateAsync(ReviewRequest request, CancellationToken ct);
		public Task<ReviewResponse> UpdateAsync(int id, ReviewRequest request, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);
	}
}
