using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CustomerController : ControllerBase
	{
		public readonly ICustomerService _customerService;
		public readonly IAddressRepository _addressRepository;

		public CustomerController(ICustomerService customerService, IAddressRepository addressRepository)
		{
			_customerService = customerService;
			_addressRepository = addressRepository;
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

				if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email))
				{
					return BadRequest("Username and email can't be empty or null");
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
					return BadRequest("Customer not created");
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
				var customer = await _customerService.GetAsync(id, ct);

				if (customer == null)
				{
					return NotFound("Customer not found");
				}

				var updatedCustomer = await _customerService.UpdateAsync(id, request, ct);

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

				await _customerService.DeleteAsync(id, ct);

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
