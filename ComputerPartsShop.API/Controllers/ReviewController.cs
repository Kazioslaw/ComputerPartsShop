using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class ReviewController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<ReviewResponse>> GetReviewList()
		{
			return Ok();
		}

		[HttpGet("{id:int}")]
		public ActionResult<ReviewResponse> GetReview(int id)
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<ReviewResponse> CreateReview(ReviewRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:int}")]
		public ActionResult<ReviewResponse> UpdateReview(int id, ReviewRequest request)
		{
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public ActionResult DeleteReview(int id)
		{
			return Ok();
		}
	}
}
