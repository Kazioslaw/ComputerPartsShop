using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CustomerController : ControllerBase
	{
		public readonly IService<CustomerRequest, CustomerResponse, DetailedCustomerResponse, Guid> _customerService;

		public CustomerController(IService<CustomerRequest, CustomerResponse, DetailedCustomerResponse, Guid> customerService)
		{
			_customerService = customerService;
		}

		[HttpGet]
		public async Task<ActionResult<List<CustomerResponse>>> GetCustomerList()
		{
			var customerList = await _customerService.GetList();

			return Ok(customerList);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<DetailedCustomerResponse>> GetCustomer(Guid id)
		{
			var customer = await _customerService.Get(id);

			if (customer == null)
			{
				return NotFound();
			}

			return Ok(customer);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerResponse>> CreateCustomer(CustomerRequest request)
		{
			var customer = await _customerService.Create(request);

			return CreatedAtAction(nameof(CreateCustomer), customer);
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<CustomerResponse>> UpdateCustomer(Guid id, CustomerRequest request)
		{
			var customer = await _customerService.Get(id);

			if (customer == null)
			{
				return NotFound();
			}

			var updatedCustomer = await _customerService.Update(id, request);

			return Ok(updatedCustomer);

		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteCustomer(Guid id)
		{
			var customer = await _customerService.Get(id);

			if (customer == null)
			{
				return NotFound();
			}

			await _customerService.Delete(id);

			return Ok();
		}
	}
}
