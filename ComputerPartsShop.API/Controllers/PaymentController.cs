using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly PaymentService _paymentService;

		public PaymentController(PaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[HttpGet]
		public async Task<ActionResult<List<PaymentResponse>>> GetPaymentList()
		{
			var paymentList = await _paymentService.GetList();

			return Ok(paymentList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedPaymentResponse>> GetPayment(int id)
		{
			var payment = await _paymentService.Get(id);

			return Ok(payment);
		}

		[HttpPost]
		public async Task<ActionResult<PaymentResponse>> CreatePayment(PaymentRequest request)
		{
			var payment = await _paymentService.Create(request);

			return CreatedAtAction(nameof(CreatePayment), payment);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<PaymentResponse>> UpdatePayment(int id, PaymentRequest request)
		{
			var payment = await _paymentService.Get(id);

			if (payment == null)
			{
				return NotFound();
			}

			var updatedPayment = await _paymentService.Update(id, request);

			return Ok(updatedPayment);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult<PaymentResponse>> Delete(int id)
		{
			var payment = await _paymentService.Get(id);

			if (payment == null)
			{
				return NotFound();
			}

			await _paymentService.Delete(id);

			return Ok();
		}
	}
}
