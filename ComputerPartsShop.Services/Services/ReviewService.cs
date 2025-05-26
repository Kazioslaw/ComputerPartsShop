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

		public async Task<List<ReviewResponse>> GetList()
		{
			var reviewList = await _reviewRepository.GetList();

			return reviewList.Select(r => new ReviewResponse(r.ID, r.Customer.Username, r.Product.Name, r.Rating, r.Description)).ToList();
		}

		public async Task<ReviewResponse> Get(int id)
		{
			var review = await _reviewRepository.Get(id);

			return review == null ? null! : new ReviewResponse(id, review.Customer.Username, review.Product.Name, review.Rating, review.Description);
		}

		public async Task<ReviewResponse> Create(ReviewRequest review)
		{
			Review newReview;
			Customer? customer = null;
			var product = await _productRepository.Get(review.ProductID);

			if (!string.IsNullOrWhiteSpace(review.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmail(review.Username);
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

			var reviewID = await _reviewRepository.Create(newReview);
			return new ReviewResponse(reviewID, review.Username, product.Name, review.Rating, review.Description);
		}

		public async Task<ReviewResponse> Update(int id, ReviewRequest updatedReview)
		{
			Review review;
			Customer? customer = null;
			var product = await _productRepository.Get(updatedReview.ProductID);

			if (!string.IsNullOrWhiteSpace(updatedReview.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmail(updatedReview.Username);
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

			await _reviewRepository.Update(id, review);

			return new ReviewResponse(id, updatedReview.Username, product.Name, updatedReview.Rating, updatedReview.Description);
		}

		public async Task Delete(int id)
		{
			await _reviewRepository.Delete(id);
		}
	}
}
