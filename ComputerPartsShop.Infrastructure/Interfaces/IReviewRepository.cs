using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IReviewRepository
	{
		public Task<List<Review>> GetListAsync(CancellationToken ct);
		public Task<Review> GetAsync(int id, CancellationToken ct);
		public Task<Review> CreateAsync(Review review, CancellationToken ct);
		public Task<Review> UpdateAsync(int id, Review review, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);
	}
}
