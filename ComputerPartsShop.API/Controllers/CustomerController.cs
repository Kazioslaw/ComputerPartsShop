using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CustomerController : ControllerBase
	{
		public readonly ICustomerService _customerService;

		public CustomerController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

		/// <summary>
		/// Asynchronously retrieves all customers.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of customers</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of customers</returns>
		[HttpGet]
		public async Task<ActionResult<List<CustomerResponse>>> GetCustomerListAsync(CancellationToken ct)
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
		public async Task<ActionResult<DetailedCustomerResponse>> GetCustomerAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var customer = await _customerService.GetAsync(id, ct);

				if (customer == null)
				{
					return NotFound();
				}

				return Ok(customer);
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
		[HttpPost]
		public async Task<ActionResult<CustomerResponse>> CreateCustomerAsync(CustomerRequest request, CancellationToken ct)
		{
			try
			{
				var customer = await _customerService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateCustomerAsync), customer);
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
		public async Task<ActionResult<CustomerResponse>> UpdateCustomerAsync(Guid id, CustomerRequest request, CancellationToken ct)
		{
			try
			{
				var customer = await _customerService.GetAsync(id, ct);

				if (customer == null)
				{
					return NotFound();
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
		/// <response code="200">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the customer was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteCustomerAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var customer = await _customerService.GetAsync(id, ct);

				if (customer == null)
				{
					return NotFound();
				}

				await _customerService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
