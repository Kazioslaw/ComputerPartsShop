using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class PaymentController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<PaymentResponse>> GetPaymentList()
		{
			return Ok();
		}

		[HttpGet("{id:Guid}")]
		public ActionResult<DetailedPaymentResponse> GetPayment(Guid id)
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<PaymentResponse> CreatePayment(PaymentRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:guid}")]
		public ActionResult<PaymentResponse> UpdatePayment(Guid id, PaymentRequest request)
		{
			return Ok();
		}

		[HttpDelete("{id:guid}")]
		public ActionResult<PaymentResponse> Delete(Guid id)
		{
			return Ok();
		}
	}
}
