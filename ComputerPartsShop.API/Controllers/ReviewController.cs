using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class ReviewController : ControllerBase
	{

		private readonly IReviewService _reviewService;
		private readonly IProductService _productService;

		public ReviewController(IReviewService reviewService, IProductService productService)
		{
			_reviewService = reviewService;
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<List<ReviewResponse>>> GetReviewListAsync()
		{
			var reviewList = await _reviewService.GetListAsync();

			return Ok(reviewList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ReviewResponse>> GetReviewAsync(int id)
		{
			var review = await _reviewService.GetAsync(id);

			if (review == null)
			{
				return NotFound();
			}

			return Ok(review);
		}

		[HttpPost]
		public async Task<ActionResult<ReviewResponse>> CreateReviewAsync(ReviewRequest request)
		{
			var product = _productService.GetAsync(request.ProductID);

			if (product == null)
			{
				return BadRequest();
			}

			var review = await _reviewService.CreateAsync(request);

			return CreatedAtAction(nameof(CreateReviewAsync), review);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<ReviewResponse>> UpdateReviewAsync(int id, ReviewRequest request)
		{
			var review = await _reviewService.GetAsync(id);

			if (review == null)
			{
				return NotFound();
			}

			var updatedReview = await _reviewService.UpdateAsync(id, request);

			return Ok(updatedReview);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteReviewAsync(int id)
		{
			var review = await _reviewService.GetAsync(id);

			if (review == null)
			{
				return NotFound();
			}

			await _reviewService.DeleteAsync(id);

			return Ok();
		}
	}
}
