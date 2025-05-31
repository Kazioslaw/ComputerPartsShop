using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class ReviewService : IReviewService
	{
		private readonly IReviewRepository _reviewRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IProductRepository _productRepository;

		public ReviewService(IReviewRepository reviewRepository, ICustomerRepository customerRepository, IProductRepository productRepository)
		{
			_reviewRepository = reviewRepository;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
		}

		public async Task<List<ReviewResponse>> GetListAsync(CancellationToken ct)
		{
			var reviewList = await _reviewRepository.GetListAsync(ct);

			return reviewList.Select(r => new ReviewResponse(r.Id, r.Customer?.Username, r.Product.Name, r.Rating, r.Description)).ToList();
		}

		public async Task<ReviewResponse> GetAsync(int id, CancellationToken ct)
		{
			var review = await _reviewRepository.GetAsync(id, ct);

			if (review == null)
			{
				return null;
			}

			return new ReviewResponse(id, review.Customer?.Username, review.Product.Name, review.Rating, review.Description);
		}

		public async Task<ReviewResponse> CreateAsync(ReviewRequest entity, CancellationToken ct)
		{
			Review newReview;
			Customer? customer = null;
			var product = await _productRepository.GetAsync(entity.ProductId, ct);

			if (!string.IsNullOrWhiteSpace(entity.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username, ct);
			}

			if (customer == null)
			{
				newReview = new()
				{
					ProductId = product.Id,
					Product = product,
					Rating = entity.Rating,
					Description = entity.Description,
				};
			}

			else
			{
				newReview = new()
				{
					CustomerId = customer.Id,
					Customer = customer,
					ProductId = product.Id,
					Product = product,
					Rating = entity.Rating,
					Description = entity.Description,
				};
			}

			var review = await _reviewRepository.CreateAsync(newReview, ct);
			return new ReviewResponse(review.Id, entity.Username, product.Name, entity.Rating, entity.Description);
		}

		public async Task<ReviewResponse> UpdateAsync(int id, ReviewRequest entity, CancellationToken ct)
		{
			Review review;
			Customer? customer = null;
			var product = await _productRepository.GetAsync(entity.ProductId, ct);

			if (!string.IsNullOrWhiteSpace(entity.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username, ct);
			}

			if (customer == null)
			{
				review = new()
				{
					ProductId = product.Id,
					Product = product,
					Rating = entity.Rating,
					Description = entity.Description,
				};
			}

			else
			{
				review = new()
				{
					CustomerId = customer.Id,
					Customer = customer,
					ProductId = product.Id,
					Product = product,
					Rating = entity.Rating,
					Description = entity.Description,
				};
			}

			await _reviewRepository.UpdateAsync(id, review, ct);

			return new ReviewResponse(id, entity.Username, product.Name, entity.Rating, entity.Description);
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _reviewRepository.DeleteAsync(id, ct);
		}
	}
}
