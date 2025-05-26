using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[HttpGet]
		public async Task<ActionResult<List<PaymentResponse>>> GetPaymentListAsync()
		{
			var paymentList = await _paymentService.GetListAsync();

			return Ok(paymentList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedPaymentResponse>> GetPaymentAsync(int id)
		{
			var payment = await _paymentService.GetAsync(id);

			return Ok(payment);
		}

		[HttpPost]
		public async Task<ActionResult<PaymentResponse>> CreatePaymentAsync(PaymentRequest request)
		{
			var payment = await _paymentService.CreateAsync(request);

			return CreatedAtAction(nameof(CreatePaymentAsync), payment);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<PaymentResponse>> UpdatePaymentAsync(int id, PaymentRequest request)
		{
			var payment = await _paymentService.GetAsync(id);

			if (payment == null)
			{
				return NotFound();
			}

			var updatedPayment = await _paymentService.UpdateAsync(id, request);

			return Ok(updatedPayment);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult<PaymentResponse>> DeleteAsync(int id)
		{
			var payment = await _paymentService.GetAsync(id);

			if (payment == null)
			{
				return NotFound();
			}

			await _paymentService.DeleteAsync(id);

			return Ok();
		}
	}
}
