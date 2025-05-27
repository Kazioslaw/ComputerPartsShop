using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PaymentProviderController : ControllerBase
	{

		private readonly IPaymentProviderService _ppService;

		public PaymentProviderController(IPaymentProviderService ppService)
		{
			_ppService = ppService;
		}

		/// <summary>
		/// Asynchronously retrieves all payment providers.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of payment providers</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of payment providers</returns>
		[HttpGet]
		public async Task<ActionResult<List<PaymentProviderResponse>>> GetPaymentProviderListAsync(CancellationToken ct)
		{
			try
			{
				var paymentProviderList = await _ppService.GetListAsync(ct);

				return Ok(paymentProviderList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an payment provider by its ID.
		/// </summary>
		/// <param name="id">Payment provider ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the payment provider</response>
		/// <response code="404">Returns if the payment provider was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Payment provider</returns>
		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedPaymentProviderResponse>> GetPaymentProviderAsync(int id, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _ppService.GetAsync(id, ct);

				if (paymentProvider == null)
				{
					return NotFound("Payment provider not found");
				}

				return Ok(paymentProvider);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new payment provider.
		/// </summary>
		/// <param name="request">Payment provider model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created payment provider</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Created payment provider</returns>
		[HttpPost]
		public async Task<ActionResult<PaymentProviderResponse>> CreatePaymentProviderAsync(PaymentProviderRequest request, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _ppService.CreateAsync(request, ct);

				return Created(nameof(CreatePaymentProviderAsync), paymentProvider);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an payment provider by its ID.
		/// </summary>
		/// <param name="id">Payment provider ID</param>
		/// <param name="request">Updated payment provider model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated payment provider</response>
		/// <response code="404">Returns if the payment provider was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated payment provider</returns>
		[HttpPut("{id:int}")]
		public async Task<ActionResult<PaymentProviderResponse>> UpdatePaymentProviderAsync(int id, PaymentProviderRequest request, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _ppService.GetAsync(id, ct);

				if (paymentProvider == null)
				{
					return NotFound("Payment provider not found");
				}

				var updatedPaymentProvider = await _ppService.UpdateAsync(id, request, ct);

				return Ok(updatedPaymentProvider);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously deletes an payment provider by its ID.
		/// </summary>
		/// <param name="id">Payment provider ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the payment provider was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeletePaymentProviderAsync(int id, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _ppService.GetAsync(id, ct);

				if (paymentProvider == null)
				{
					return NotFound("Payment provider not found");
				}

				await _ppService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
