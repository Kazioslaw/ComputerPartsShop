using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IReviewRepository
	{
		public Task<List<Review>> GetListAsync(CancellationToken ct);
		public Task<Review> GetAsync(int id, CancellationToken ct);
		public Task<Review> CreateAsync(Review request, CancellationToken ct);
		public Task<Review> UpdateAsync(int id, Review request, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);
	}
}
