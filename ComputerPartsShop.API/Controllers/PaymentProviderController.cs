using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Authorize(Roles = "Admin")]
	[Route("[controller]")]
	public class PaymentProviderController : ControllerBase
	{

		private readonly IPaymentProviderService _paymentProviderService;
		private readonly IValidator<PaymentProviderRequest> _paymentProviderValidator;


		public PaymentProviderController(IPaymentProviderService paymentProviderService, IValidator<PaymentProviderRequest> paymentProviderValidator)
		{
			_paymentProviderService = paymentProviderService;
			_paymentProviderValidator = paymentProviderValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all payment providers.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of payment providers</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>List of payment providers</returns>
		[HttpGet]
		public async Task<IActionResult> GetPaymentProviderListAsync(CancellationToken ct)
		{
			try
			{
				var paymentProviderList = await _paymentProviderService.GetListAsync(ct);

				return Ok(paymentProviderList);
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
		/// Asynchronously retrieves an payment provider by its ID.
		/// </summary>
		/// <param name="id">Payment provider ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the payment provider</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the payment provider was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Payment provider</returns>
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetPaymentProviderAsync(int id, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _paymentProviderService.GetAsync(id, ct);

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
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an payment provider by its name
		/// </summary>
		/// <param name="name">Payment provider name</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the payment provider</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the payment provider was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Payment provider</returns>
		[HttpGet("{name}")]
		public async Task<IActionResult> GetPaymentProviderByNameAsync(string name, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _paymentProviderService.GetByNameAsync(name, ct);

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
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously creates a new payment provider.
		/// </summary>
		/// <param name="request">Payment provider model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created payment provider</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Created payment provider</returns>
		[HttpPost]
		public async Task<IActionResult> CreatePaymentProviderAsync(PaymentProviderRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _paymentProviderValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var paymentProvider = await _paymentProviderService.CreateAsync(request, ct);

				return Created(nameof(GetPaymentProviderAsync), paymentProvider);
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
		/// Asynchronously updates an payment provider by its ID.
		/// </summary>
		/// <param name="id">Payment provider ID</param>
		/// <param name="request">Updated payment provider model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated payment provider</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the payment provider was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated payment provider</returns>
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdatePaymentProviderAsync(int id, PaymentProviderRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _paymentProviderValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var updatedPaymentProvider = await _paymentProviderService.UpdateAsync(id, request, ct);

				return Ok(updatedPaymentProvider);
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
		/// Asynchronously deletes an payment provider by its ID.
		/// </summary>
		/// <param name="id">Payment provider ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the payment provider was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeletePaymentProviderAsync(int id, CancellationToken ct)
		{
			try
			{
				await _paymentProviderService.DeleteAsync(id, ct);

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
