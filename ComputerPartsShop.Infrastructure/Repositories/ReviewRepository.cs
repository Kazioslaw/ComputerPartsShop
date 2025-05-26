using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class ReviewRepository : IReviewRepository
	{
		readonly TempData _dbContext;

		public ReviewRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Review>> GetListAsync()
		{
			return _dbContext.ReviewList;
		}

		public async Task<Review> GetAsync(int id)
		{
			var review = _dbContext.ReviewList.FirstOrDefault(x => x.ID == id);

			return review;
		}

		public async Task<int> CreateAsync(Review request)
		{
			var last = _dbContext.ReviewList.LastOrDefault();

			if (last == null)
			{
				request.ID = 1;
			}
			else
			{
				request.ID = last.ID + 1;
			}

			_dbContext.ReviewList.Add(request);

			return request.ID;
		}

		public async Task<Review> UpdateAsync(int id, Review request)
		{
			var review = _dbContext.ReviewList.FirstOrDefault(x => x.ID == id);

			if (review != null)
			{
				review.CustomerID = request.CustomerID;
				review.ProductID = request.ProductID;
				review.Rating = request.Rating;
				review.Description = request.Description;
			}

			return review;
		}

		public async Task DeleteAsync(int id)
		{
			var review = _dbContext.ReviewList.FirstOrDefault(x => x.ID == id);

			if (review != null)
			{
				_dbContext.ReviewList.Remove(review);
			}
		}
	}
}
