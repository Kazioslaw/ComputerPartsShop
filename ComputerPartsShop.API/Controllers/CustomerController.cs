using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CustomerController : ControllerBase
	{
		public readonly ICustomerService _customerService;

		public CustomerController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

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
