using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;
		private readonly ICustomerPaymentSystemService _customerPaymentSystemService;

		public PaymentController(IPaymentService paymentService, ICustomerPaymentSystemService customerPaymentSystemService)
		{
			_paymentService = paymentService;
			_customerPaymentSystemService = customerPaymentSystemService;
		}

		/// <summary>
		/// Asynchronously retrieves all payments.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of payments</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of payments</returns>
		[HttpGet]
		public async Task<IActionResult> GetPaymentListAsync(CancellationToken ct)
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

		/// <summary>
		/// Asynchronously retrieves an payment by its ID.
		/// </summary>
		/// <param name="id">Payment ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the payment</response>
		/// <response code="404">Returns if the payment was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Payment</returns>
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetPaymentAsync(Guid id, CancellationToken ct)
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

		/// <summary>
		/// Asynchronously creates a new payment.
		/// </summary>
		/// <param name="request">Payment model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created payment</response>
		/// <response code="400">Returns if the Customer payment system id was invalid</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Created payment</returns>
		[HttpPost]
		public async Task<IActionResult> CreatePaymentAsync(PaymentRequest request, CancellationToken ct)
		{
			try
			{
				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(request.CustomerPaymentSystemId, ct);

				if (customerPaymentSystem == null)
				{
					return BadRequest("Invalid customer payment system ID");
				}

				var payment = await _paymentService.CreateAsync(request, ct);

				return Created(nameof(GetPaymentAsync), payment);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an payment by its ID.
		/// </summary>
		/// <param name="id">Payment ID</param>
		/// <param name="request">Updated payment model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated payment</response>
		/// <response code="400">Returns if the customer payment system id was invalid</response>
		/// <response code="404">Returns if the payment was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated payment</returns>
		[HttpPut("{id:guid}")]
		public async Task<IActionResult> UpdatePaymentAsync(Guid id, UpdatePaymentRequest request, CancellationToken ct)
		{
			try
			{
				var payment = await _paymentService.GetAsync(id, ct);

				if (payment == null)
				{
					return NotFound("Payment not found");
				}

				var updatedPayment = await _paymentService.UpdateStatusAsync(id, request, ct);

				return Ok(updatedPayment);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously deletes an payment by its ID.
		/// </summary>
		/// <param name="id">Payment ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the payment was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var payment = await _paymentService.GetAsync(id, ct);

				if (payment == null)
				{
					return NotFound("Payment not found");
				}

				var isDeleted = await _paymentService.DeleteAsync(id, ct);

				if (!isDeleted)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Delete failed");
				}

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
