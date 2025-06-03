using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CustomerController : ControllerBase
	{
		public readonly ICustomerService _customerService;
		public readonly IAddressRepository _addressRepository;
		public readonly IValidator<CustomerRequest> _customerValidator;

		public CustomerController(ICustomerService customerService, IAddressRepository addressRepository, IValidator<CustomerRequest> customerValidator)
		{
			_customerService = customerService;
			_addressRepository = addressRepository;
			_customerValidator = customerValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all customers.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of customers</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of customers</returns>
		[HttpGet]
		public async Task<IActionResult> GetCustomerListAsync(CancellationToken ct)
		{
			try
			{
				var customerList = await _customerService.GetListAsync(ct);

				return Ok(customerList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an customer by its ID.
		/// </summary>
		/// <param name="id">Customer ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the customer</response>
		/// <response code="404">Returns if the customer was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Customer</returns>
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetCustomerAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var customer = await _customerService.GetAsync(id, ct);

				if (customer == null)
				{
					return NotFound("Customer not found");
				}

				return Ok(customer);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an customer by its Username or Email.
		/// </summary>
		/// <param name="usernameOrEmail">Customer Username or Email</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the customer</response>
		/// <response code="404">Returns if the customer was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Customer</returns>
		[HttpGet("{usernameOrEmail}")]
		public async Task<IActionResult> GetCustomerByUsernameOrEmail(string usernameOrEmail, CancellationToken ct)
		{
			try
			{
				var customer = await _customerService.GetByUsernameOrEmailAsync(usernameOrEmail, ct);

				if (customer == null)
				{
					return NotFound("Customer not found");
				}

				return Ok(customer);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new customer.
		/// </summary>
		/// <param name="request">Customer model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the customer</response>
		/// <response code="400">Returns if the customer was not created</response>
		/// <response code="404">Returns if the customer was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Customer</returns>
		[HttpPost]
		public async Task<IActionResult> CreateCustomerAsync(CustomerRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _customerValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var existingUsername = await _customerService.GetByUsernameOrEmailAsync(request.Username, ct);
				var existingEmail = await _customerService.GetByUsernameOrEmailAsync(request.Email, ct);

				if (existingUsername != null || existingEmail != null)
				{
					return BadRequest("That username or email is exist");
				}

				var customer = await _customerService.CreateAsync(request, ct);

				if (customer == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Create failed");
				}

				return Ok(customer);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an customer by its ID.
		/// </summary>
		/// <param name="id">Customer ID</param>
		/// <param name="request">Updated customer model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated customer</response>
		/// <response code="404">Returns if the customer was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated customer</returns>
		[HttpPut("{id:guid}")]
		public async Task<IActionResult> UpdateCustomerAsync(Guid id, CustomerRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _customerValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var customer = await _customerService.GetAsync(id, ct);

				if (customer == null)
				{
					return NotFound("Customer not found");
				}

				var updatedCustomer = await _customerService.UpdateAsync(id, request, ct);

				if (updatedCustomer == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");
				}

				return Ok(updatedCustomer);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously deletes an customer by its ID.
		/// </summary>
		/// <param name="id">Customer ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the customer was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteCustomerAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var customer = await _customerService.GetAsync(id, ct);

				if (customer == null)
				{
					return NotFound("Customer not found");
				}

				var isDeleted = await _customerService.DeleteAsync(id, ct);

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
