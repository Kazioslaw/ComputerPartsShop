using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;

namespace ComputerPartsShop.Services
{
	public class ReviewService : IReviewService
	{
		private readonly IReviewRepository _reviewRepository;
		private readonly IShopUserRepository _userRepository;
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public ReviewService(IReviewRepository reviewRepository, IShopUserRepository userRepository, IProductRepository productRepository, IMapper mapper)
		{
			_reviewRepository = reviewRepository;
			_userRepository = userRepository;
			_productRepository = productRepository;
			_mapper = mapper;

		}

		public async Task<List<ReviewResponse>> GetListAsync(CancellationToken ct)
		{
			try
			{
				var result = await _reviewRepository.GetListAsync(ct);

				var reviewList = _mapper.Map<IEnumerable<ReviewResponse>>(result);

				return reviewList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ReviewResponse> GetAsync(int id, CancellationToken ct)
		{
			try
			{
				var result = await _reviewRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "Review not found");
				}

				var review = _mapper.Map<ReviewResponse>(result);

				return review;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ReviewResponse> CreateAsync(ReviewRequest entity, CancellationToken ct)
		{
			try
			{
				var product = await _productRepository.GetAsync(entity.ProductId, ct);

				if (product == null)
				{
					throw new DataErrorException(400, "Invalid product id");
				}

				ShopUser? user = null;

				if (!string.IsNullOrWhiteSpace(entity.Username))
				{
					user = await _userRepository.GetByUsernameOrEmailAsync(entity.Username, ct);
				}

				var newReview = _mapper.Map<Review>(entity);
				newReview.Product = product;
				if (user != null)
				{
					newReview.User = user;
					newReview.UserId = user.Id;
				}

				var result = await _reviewRepository.CreateAsync(newReview, ct);

				var createdReview = _mapper.Map<ReviewResponse>(result);

				return createdReview;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ReviewResponse> UpdateAsync(int id, ReviewRequest entity, CancellationToken ct)
		{
			try
			{
				var review = await _reviewRepository.GetAsync(id, ct);

				if (review == null)
				{
					throw new DataErrorException(404, "Review not found");
				}

				var product = await _productRepository.GetAsync(entity.ProductId, ct);

				if (product == null)
				{
					throw new DataErrorException(400, "Invalid product id");
				}

				var reviewToUpdate = _mapper.Map<Review>(entity);
				ShopUser? user = null;

				if (!string.IsNullOrWhiteSpace(entity.Username))
				{
					user = await _userRepository.GetByUsernameOrEmailAsync(entity.Username, ct);
				}

				reviewToUpdate.Product = product;
				reviewToUpdate.ProductId = product.Id;

				if (user != null)
				{
					reviewToUpdate.User = user;
					reviewToUpdate.UserId = user.Id;
				}

				var result = await _reviewRepository.UpdateAsync(id, reviewToUpdate, ct);

				var updatedReview = _mapper.Map<ReviewResponse>(result);

				return updatedReview;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			try
			{
				var review = await _reviewRepository.GetAsync(id, ct);

				if (review == null)
				{
					throw new DataErrorException(404, "Review not found");
				}

				await _reviewRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}
	}
}
