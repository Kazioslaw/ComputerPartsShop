using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class ReviewController : ControllerBase
	{

		private readonly ReviewService _reviewService;
		private readonly ProductService _productService;

		public ReviewController(ReviewService reviewService, ProductService productService)
		{
			_reviewService = reviewService;
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<List<ReviewResponse>>> GetReviewList()
		{
			var reviewList = await _reviewService.GetList();

			return Ok(reviewList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ReviewResponse>> GetReview(int id)
		{
			var review = await _reviewService.Get(id);

			if (review == null)
			{
				return NotFound();
			}

			return Ok(review);
		}

		[HttpPost]
		public async Task<ActionResult<ReviewResponse>> CreateReview(ReviewRequest request)
		{
			var product = _productService.Get(request.ProductID);

			if (product == null)
			{
				return BadRequest();
			}

			var review = await _reviewService.Create(request);

			return CreatedAtAction(nameof(CreateReview), review);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<ReviewResponse>> UpdateReview(int id, ReviewRequest request)
		{
			var review = await _reviewService.Get(id);

			if (review == null)
			{
				return NotFound();
			}

			var updatedReview = await _reviewService.Update(id, request);

			return Ok(updatedReview);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteReview(int id)
		{
			var review = await _reviewService.Get(id);

			if (review == null)
			{
				return NotFound();
			}

			await _reviewService.Delete(id);

			return Ok();
		}
	}
}
