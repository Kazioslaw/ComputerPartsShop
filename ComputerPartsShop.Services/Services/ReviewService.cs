using AutoMapper;
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
		private readonly IMapper _mapper;

		public ReviewService(IReviewRepository reviewRepository, ICustomerRepository customerRepository, IProductRepository productRepository, IMapper mapper)
		{
			_reviewRepository = reviewRepository;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
			_mapper = mapper;

		}

		public async Task<List<ReviewResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _reviewRepository.GetListAsync(ct);

			var reviewList = _mapper.Map<IEnumerable<ReviewResponse>>(result);

			return reviewList.ToList();
		}

		public async Task<ReviewResponse> GetAsync(int id, CancellationToken ct)
		{
			var result = await _reviewRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var review = _mapper.Map<ReviewResponse>(result);

			return review;
		}

		public async Task<ReviewResponse> CreateAsync(ReviewRequest entity, CancellationToken ct)
		{
			Customer? customer = null;
			var product = await _productRepository.GetAsync(entity.ProductId, ct);

			if (!string.IsNullOrWhiteSpace(entity.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username, ct);
			}

			var newReview = _mapper.Map<Review>(entity);
			newReview.Product = product;
			if (customer != null)
			{
				newReview.Customer = customer;
				newReview.CustomerId = customer.Id;
			}

			var result = await _reviewRepository.CreateAsync(newReview, ct);

			if (result == null)
			{
				return null;
			}

			var createdReview = _mapper.Map<ReviewResponse>(result);

			return createdReview;
		}

		public async Task<ReviewResponse> UpdateAsync(int id, ReviewRequest entity, CancellationToken ct)
		{
			var reviewToUpdate = _mapper.Map<Review>(entity);
			Customer? customer = null;
			var product = await _productRepository.GetAsync(entity.ProductId, ct);

			if (!string.IsNullOrWhiteSpace(entity.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username, ct);
			}

			reviewToUpdate.Product = product;
			reviewToUpdate.ProductId = product.Id;

			if (customer != null)
			{
				reviewToUpdate.Customer = customer;
				reviewToUpdate.CustomerId = customer.Id;
			}

			var result = await _reviewRepository.UpdateAsync(id, reviewToUpdate, ct);

			if (result == null)
			{
				return null;
			}

			var updatedReview = _mapper.Map<ReviewResponse>(result);

			return updatedReview;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _reviewRepository.DeleteAsync(id, ct);
		}
	}
}
