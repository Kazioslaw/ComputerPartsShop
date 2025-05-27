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

		/// <summary>
		/// Asynchronously retrieves an payment by its ID.
		/// </summary>
		/// <param name="id">Payment ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the payment</response>
		/// <response code="404">Returns if the payment was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Payment</returns>
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
		public async Task<ActionResult<PaymentResponse>> CreatePaymentAsync(PaymentRequest request, CancellationToken ct)
		{
			try
			{
				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(request.CustomerPaymentSystemId, ct);

				if (customerPaymentSystem == null)
				{
					return BadRequest("Invalid customer payment system ID");
				}

				var payment = await _paymentService.CreateAsync(request, ct);

				return Created(nameof(CreatePaymentAsync), payment);
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
		[HttpPut("{id:int}")]
		public async Task<ActionResult<PaymentResponse>> UpdatePaymentAsync(int id, PaymentRequest request, CancellationToken ct)
		{
			try
			{
				var payment = await _paymentService.GetAsync(id, ct);

				if (payment == null)
				{
					return NotFound("Payment not found");
				}

				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(request.CustomerPaymentSystemId, ct);

				if (customerPaymentSystem == null)
				{
					return BadRequest("Invalid customer payment system ID");
				}

				var updatedPayment = await _paymentService.UpdateAsync(id, request, ct);

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
		/// <response code="200">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the payment was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		public async Task<ActionResult<PaymentResponse>> DeleteAsync(int id, CancellationToken ct)
		{
			try
			{
				var payment = await _paymentService.GetAsync(id, ct);

				if (payment == null)
				{
					return NotFound("Payment not found");
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
