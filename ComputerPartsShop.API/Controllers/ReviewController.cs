using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ReviewController : ControllerBase
	{

		private readonly IReviewService _reviewService;
		private readonly IProductService _productService;

		public ReviewController(IReviewService reviewService, IProductService productService)
		{
			_reviewService = reviewService;
			_productService = productService;
		}

		/// <summary>
		/// Asynchronously retrieves all reviews.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of reviews</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of reviews</returns>
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
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an review by its ID.
		/// </summary>
		/// <param name="id">Review ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the review</response>
		/// <response code="404">Returns if the review was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Review</returns>
		[HttpGet("{id:int}")]
		public async Task<ActionResult<ReviewResponse>> GetReviewAsync(int id, CancellationToken ct)
		{
			try
			{
				var review = await _reviewService.GetAsync(id, ct);

				if (review == null)
				{
					return NotFound("Review not found");
				}

				return Ok(review);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new review.
		/// </summary>
		/// <param name="request">Review model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created review</response>
		/// <response code="400">Returns if the product id was invalid</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Created review</returns>
		[HttpPost]
		public async Task<ActionResult<ReviewResponse>> CreateReviewAsync(ReviewRequest request, CancellationToken ct)
		{
			try
			{
				var product = await _productService.GetAsync(request.ProductId, ct);


				if (product == null)
				{
					return BadRequest("Invalid product ID");
				}

				var review = await _reviewService.CreateAsync(request, ct);

				return Created(nameof(CreateReviewAsync), review);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an review by its ID.
		/// </summary>
		/// <param name="id">Review ID</param>
		/// <param name="request">Updated review model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated review</response>
		/// <response code="400">Returns if the product id was invalid</response>
		/// <response code="404">Returns if the review was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated review</returns>
		[HttpPut("{id:int}")]
		public async Task<ActionResult<ReviewResponse>> UpdateReviewAsync(int id, ReviewRequest request, CancellationToken ct)
		{
			try
			{
				var review = await _reviewService.GetAsync(id, ct);

				if (review == null)
				{
					return NotFound("Review not found");
				}

				var product = await _productService.GetAsync(request.ProductId, ct);

				if (product == null)
				{
					return BadRequest("Invalid product ID");
				}

				var updatedReview = await _reviewService.UpdateAsync(id, request, ct);

				return Ok(updatedReview);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously deletes an review by its ID.
		/// </summary>
		/// <param name="id">Review ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the review was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteReviewAsync(int id, CancellationToken ct)
		{
			try
			{
				var review = await _reviewService.GetAsync(id, ct);

				if (review == null)
				{
					return NotFound("Review not found");
				}

				await _reviewService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
