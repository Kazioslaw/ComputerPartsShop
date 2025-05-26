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
		public async Task<ActionResult<List<ReviewResponse>>> GetReviewListAsync(CancellationToken ct)
		{
			try
			{
				var reviewList = await _reviewService.GetListAsync(ct);

				return Ok(reviewList);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ReviewResponse>> GetReviewAsync(int id, CancellationToken ct)
		{
			try
			{
				var review = await _reviewService.GetAsync(id, ct);

				if (review == null)
				{
					return NotFound();
				}

				return Ok(review);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<ActionResult<ReviewResponse>> CreateReviewAsync(ReviewRequest request, CancellationToken ct)
		{
			try
			{
				var product = _productService.GetAsync(request.ProductID, ct);

				if (product == null)
				{
					return BadRequest();
				}

				var review = await _reviewService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateReviewAsync), review);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<ReviewResponse>> UpdateReviewAsync(int id, ReviewRequest request, CancellationToken ct)
		{
			try
			{
				var review = await _reviewService.GetAsync(id, ct);

				if (review == null)
				{
					return NotFound();
				}

				var updatedReview = await _reviewService.UpdateAsync(id, request, ct);

				return Ok(updatedReview);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteReviewAsync(int id, CancellationToken ct)
		{
			try
			{
				var review = await _reviewService.GetAsync(id, ct);

				if (review == null)
				{
					return NotFound();
				}

				await _reviewService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}
	}
}
