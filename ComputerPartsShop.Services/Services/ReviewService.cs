using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class ReviewService : IService<ReviewRequest, ReviewResponse, ReviewResponse, int>
	{
		private readonly IRepository<Review, int> _reviewRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IRepository<Product, int> _productRepository;

		public ReviewService(IRepository<Review, int> reviewRepository, ICustomerRepository customerRepository, IRepository<Product, int> productRepository)
		{
			_reviewRepository = reviewRepository;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
		}

		public async Task<List<ReviewResponse>> GetListAsync()
		{
			var reviewList = await _reviewRepository.GetListAsync();

			return reviewList.Select(r => new ReviewResponse(r.ID, r.Customer.Username, r.Product.Name, r.Rating, r.Description)).ToList();
		}

		public async Task<ReviewResponse> GetAsync(int id)
		{
			var review = await _reviewRepository.GetAsync(id);

			return review == null ? null! : new ReviewResponse(id, review.Customer.Username, review.Product.Name, review.Rating, review.Description);
		}

		public async Task<ReviewResponse> CreateAsync(ReviewRequest review)
		{
			Review newReview;
			Customer? customer = null;
			var product = await _productRepository.GetAsync(review.ProductID);

			if (!string.IsNullOrWhiteSpace(review.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(review.Username);
			}

			if (customer == null)
			{
				newReview = new()
				{
					ProductID = product.ID,
					Product = product,
					Rating = review.Rating,
					Description = review.Description,
				};
			}

			else
			{
				newReview = new()
				{
					CustomerID = customer.ID,
					Customer = customer,
					ProductID = product.ID,
					Product = product,
					Rating = review.Rating,
					Description = review.Description,
				};
			}

			var reviewID = await _reviewRepository.CreateAsync(newReview);
			return new ReviewResponse(reviewID, review.Username, product.Name, review.Rating, review.Description);
		}

		public async Task<ReviewResponse> UpdateAsync(int id, ReviewRequest updatedReview)
		{
			Review review;
			Customer? customer = null;
			var product = await _productRepository.GetAsync(updatedReview.ProductID);

			if (!string.IsNullOrWhiteSpace(updatedReview.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(updatedReview.Username);
			}

			if (customer == null)
			{
				review = new()
				{
					ProductID = product.ID,
					Product = product,
					Rating = updatedReview.Rating,
					Description = updatedReview.Description,
				};
			}

			else
			{
				review = new()
				{
					CustomerID = customer.ID,
					Customer = customer,
					ProductID = product.ID,
					Product = product,
					Rating = updatedReview.Rating,
					Description = updatedReview.Description,
				};
			}

			await _reviewRepository.UpdateAsync(id, review);

			return new ReviewResponse(id, updatedReview.Username, product.Name, updatedReview.Rating, updatedReview.Description);
		}

		public async Task DeleteAsync(int id)
		{
			await _reviewRepository.DeleteAsync(id);
		}
	}
}
