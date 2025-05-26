using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class ReviewRepository : IReviewRepository
	{
		private readonly TempData _dbContext;

		public ReviewRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Review>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);

			return _dbContext.ReviewList;
		}

		public async Task<Review> GetAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var review = _dbContext.ReviewList.FirstOrDefault(x => x.Id == id);

			return review!;
		}

		public async Task<int> CreateAsync(Review request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var last = _dbContext.ReviewList.LastOrDefault();

			if (last == null)
			{
				request.Id = 1;
			}
			else
			{
				request.Id = last.Id + 1;
			}

			_dbContext.ReviewList.Add(request);

			return request.Id;
		}

		public async Task<Review> UpdateAsync(int id, Review request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var review = _dbContext.ReviewList.FirstOrDefault(x => x.Id == id);

			if (review != null)
			{
				review.CustomerId = request.CustomerId;
				review.ProductId = request.ProductId;
				review.Rating = request.Rating;
				review.Description = request.Description;
			}

			return review!;
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var review = _dbContext.ReviewList.FirstOrDefault(x => x.Id == id);

			if (review != null)
			{
				_dbContext.ReviewList.Remove(review);
			}
		}
	}
}
