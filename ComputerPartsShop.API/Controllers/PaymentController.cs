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
		public async Task<ActionResult<List<PaymentResponse>>> GetPaymentListAsync(CancellationToken ct)
		{
			try
			{
				var paymentList = await _paymentService.GetListAsync(ct);

				return Ok(paymentList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedPaymentResponse>> GetPaymentAsync(int id, CancellationToken ct)
		{
			try
			{
				var payment = await _paymentService.GetAsync(id, ct);

				return Ok(payment);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpPost]
		public async Task<ActionResult<PaymentResponse>> CreatePaymentAsync(PaymentRequest request, CancellationToken ct)
		{
			try
			{
				var payment = await _paymentService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreatePaymentAsync), payment);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<PaymentResponse>> UpdatePaymentAsync(int id, PaymentRequest request, CancellationToken ct)
		{
			try
			{
				var payment = await _paymentService.GetAsync(id, ct);

				if (payment == null)
				{
					return NotFound();
				}

				var updatedPayment = await _paymentService.UpdateAsync(id, request, ct);

				return Ok(updatedPayment);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult<PaymentResponse>> DeleteAsync(int id, CancellationToken ct)
		{
			try
			{
				var payment = await _paymentService.GetAsync(id, ct);

				if (payment == null)
				{
					return NotFound();
				}

				await _paymentService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
