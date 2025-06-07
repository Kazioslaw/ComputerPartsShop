using ComputerPartsShop.Domain;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]")]
	public class ReviewController : ControllerBase
	{
		private readonly IReviewService _reviewService;
		private readonly IValidator<ReviewRequest> _reviewValidator;

		public ReviewController(IReviewService reviewService, IValidator<ReviewRequest> reviewValidator)
		{
			_reviewService = reviewService;
			_reviewValidator = reviewValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all reviews.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of reviews</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>List of reviews</returns>
		[HttpGet]
		public async Task<IActionResult> GetReviewListAsync(CancellationToken ct)
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
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an review by its ID.
		/// </summary>
		/// <param name="id">Review ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the review</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the review was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Review</returns>
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetReviewAsync(int id, CancellationToken ct)
		{
			try
			{
				var review = await _reviewService.GetAsync(id, ct);

				return Ok(review);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously creates a new review.
		/// </summary>
		/// <param name="request">Review model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created review</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="400">Returns if the product id was invalid</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Created review</returns>
		[HttpPost]
		public async Task<IActionResult> CreateReviewAsync(ReviewRequest request, CancellationToken ct)
		{
			try
			{
				var validation = _reviewValidator.Validate(request);
				var usernameFromToken = HttpContext.User.FindFirst("unique_name")?.Value;

				if (string.IsNullOrWhiteSpace(request.Username) && usernameFromToken != null)
				{
					request = request with { Username = usernameFromToken };
				}

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var review = await _reviewService.CreateAsync(request, ct);

				if (review == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Create failed");
				}

				return Ok(review);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
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
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the review was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated review</returns>
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateReviewAsync(int id, ReviewRequest request, CancellationToken ct)
		{
			try
			{

				var validation = _reviewValidator.Validate(request);
				var usernameFromToken = HttpContext.User.Identity?.Name;

				if (string.IsNullOrWhiteSpace(usernameFromToken))
				{
					throw new DataErrorException(HttpStatusCode.Forbidden, "Username is missing.");
				}

				request = request with { Username = usernameFromToken };

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var updatedReview = await _reviewService.UpdateAsync(id, request, ct);

				return Ok(updatedReview);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously deletes an review by its ID.
		/// </summary>
		/// <param name="id">Review ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the review was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> DeleteReviewAsync(int id, CancellationToken ct)
		{
			try
			{
				await _reviewService.DeleteAsync(id, ct);

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}
	}
}
