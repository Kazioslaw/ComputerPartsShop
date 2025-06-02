using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CustomerPaymentSystemController : ControllerBase
	{

		private readonly ICustomerPaymentSystemService _customerPaymentSystemService;
		private readonly IPaymentProviderRepository _providerRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IValidator<CustomerPaymentSystemRequest> _customerPaymentSystemValidator;
		public CustomerPaymentSystemController(ICustomerPaymentSystemService customerPaymentSystemService,
			IPaymentProviderRepository providerRepository, ICustomerRepository customerRepository,
			IValidator<CustomerPaymentSystemRequest> customerPaymentSystemValidator)
		{
			_customerPaymentSystemService = customerPaymentSystemService;
			_providerRepository = providerRepository;
			_customerRepository = customerRepository;
			_customerPaymentSystemValidator = customerPaymentSystemValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all customer payment systems.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of customer payment systems</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of customer payment systems</returns>

		[HttpGet]
		public async Task<IActionResult> GetCustomerPaymentSystemListAsync(CancellationToken ct)
		{
			try
			{
				var customerPaymentSystemList = await _customerPaymentSystemService.GetListAsync(ct);

				return Ok(customerPaymentSystemList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an customer payment system by its ID.
		/// </summary>
		/// <param name="id">Customer payment system ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the customer payment system</response>
		/// <response code="404">Returns if the customer payment system was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Customer payment system</returns>
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetCustomerPaymentSystemAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(id, ct);

				if (customerPaymentSystem == null)
				{
					return NotFound("Customer payment system not found");
				}

				return Ok(customerPaymentSystem);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new customer payment system.
		/// </summary>
		/// <param name="request">Customer payment system model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created customer payment system</response>
		/// <response code="400">Returns if the username, email or provider name was empty or invalid</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Created customer payment system</returns>
		[HttpPost]
		public async Task<IActionResult> CreateCustomerPaymentSystemAsync(CustomerPaymentSystemRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _customerPaymentSystemValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var paymentProvider = await _providerRepository.GetByNameAsync(request.ProviderName, ct);
				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!, ct);

				if (paymentProvider == null)
				{
					return BadRequest("Invalid provider name");
				}

				if (customer == null)
				{
					return BadRequest("Invalid or missing username or email");
				}

				var customerPaymentSystem = await _customerPaymentSystemService.CreateAsync(request, ct);

				if (customerPaymentSystem == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Create failed");
				}

				return Created(nameof(GetCustomerPaymentSystemAsync), customerPaymentSystem);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an customer payment system by its ID.
		/// </summary>
		/// <param name="id">Customer payment system ID</param>
		/// <param name="request">Updated customer payment system model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated customer payment system</response>
		/// <response code="400">Returns if the username, email or provider name was empty or invalid</response>
		/// <response code="404">Returns if the customer payment system was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated customer payment system</returns>
		[HttpPut("{id:guid}")]
		public async Task<IActionResult> UpdateCustomerPaymentSystemAsync(Guid id, CustomerPaymentSystemRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _customerPaymentSystemValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(id, ct);
				var paymentProvider = await _providerRepository.GetByNameAsync(request.ProviderName, ct);
				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!, ct);

				if (paymentProvider == null)
				{
					return BadRequest("Invalid provider name");

				}

				if (customer == null)
				{
					return BadRequest("Invalid or missing username or email");
				}

				if (customerPaymentSystem == null)
				{
					return NotFound("Customer payment system not found");
				}

				var customerPaymentSystemUpdated = await _customerPaymentSystemService.UpdateAsync(id, request, ct);

				if (customerPaymentSystemUpdated == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");
				}

				return Ok(customerPaymentSystemUpdated);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously deletes an customer payment system by its ID.
		/// </summary>
		/// <param name="id">Customer payment system ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the customer payment system was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteCustomerPaymentSystemAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(id, ct);

				if (customerPaymentSystem == null)
				{
					return NotFound("Customer payment system not found");
				}

				var isDeleted = await _customerPaymentSystemService.DeleteAsync(id, ct);

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
