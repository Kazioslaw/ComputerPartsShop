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
		public async Task<ActionResult<List<CustomerResponse>>> GetCustomerListAsync()
		{
			var customerList = await _customerService.GetListAsync();

			return Ok(customerList);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<DetailedCustomerResponse>> GetCustomerAsync(Guid id)
		{
			var customer = await _customerService.GetAsync(id);

			if (customer == null)
			{
				return NotFound();
			}

			return Ok(customer);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerResponse>> CreateCustomerAsync(CustomerRequest request)
		{
			var customer = await _customerService.CreateAsync(request);

			return CreatedAtAction(nameof(CreateCustomerAsync), customer);
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<CustomerResponse>> UpdateCustomerAsync(Guid id, CustomerRequest request)
		{
			var customer = await _customerService.GetAsync(id);

			if (customer == null)
			{
				return NotFound();
			}

			var updatedCustomer = await _customerService.UpdateAsync(id, request);

			return Ok(updatedCustomer);

		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteCustomerAsync(Guid id)
		{
			var customer = await _customerService.GetAsync(id);

			if (customer == null)
			{
				return NotFound();
			}

			await _customerService.DeleteAsync(id);

			return Ok();
		}
	}
}
